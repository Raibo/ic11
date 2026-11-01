# Compiler validation

Run the suite from the repository root with the .NET 9 SDK:

```text
dotnet test ic11.sln -c Release
```

The suite has 151 cases, including the existing 23 upstream cases. It needs no
game installation, browser, external emulator, or new package dependencies.
The compiler, grammar, command-line interface, extension, and generated output
are unchanged by this validation work.

## What is checked

| Suite | Purpose |
| --- | --- |
| `EmulatorInstructionTests` | Execute raw IC10 against hand-specified results to exercise the test interpreter independently of the compiler. Covers unary/ternary instructions, named batch filtering, empty batches, old and unified ReferenceId addressing, missing devices, unsupported instructions and halt behavior. |
| `CompilerBehaviorTests` | Compile IC11 and check independently expected values. Covers constant-folded and device-input expressions, numeric boundaries, nested expression operands, eight variables kept live across nested calls, stack restoration, branches, break/continue, and every element of a sorted array with duplicates and negatives. |
| `DeviceAccessTests` | Supply controlled reads and check output targets, values and operation order. Covers aliases and reconnection, indexed pins, disconnected-pin guards, computed ReferenceIds, slot indices, named batches, all four batch-read forms, slot batches, device memory and reagent operands. |

The new compiler cases use `CompilerTestHarness`. It rejects syntax errors in
fixtures before compiling them and fails when a finite fixture exceeds its tick
budget. Some cases use one instruction per tick to exercise suspension while
intermediate values and call frames are live, alongside the normal budget of
128. Cases that deliberately run forever should provide their own bounded
observations instead of using `Finish`.

## Controlled device fixtures

Device fixtures are passive storage for properties, slots, memory, and reagent
values. The tests check the instructions generated to access that storage;
they do not simulate recipes, heating, movement, harvesting or other game
physics. A fixture's expected values are specified independently of compiler
helpers and constant-folding code.

Network devices are registered by ReferenceId:

```csharp
var device = emulator.RegisterDevice(1001, prefabHash: 123, nameHash: 456);
device.SetProperty("Setting", 7);
device.SetSlotProperty(2, "Quantity", 10);
```

The existing `BatchDevices` collection is now interpreted as network devices
keyed by ReferenceId, with `PrefabHash` and `NameHash` held as properties. It can
represent multiple devices sharing a type or a name. Unmatched batch writes do
nothing; a direct access to an unregistered ReferenceId fails instead of
creating a mock that could hide a targeting error.

Pin aliases resolve their current address when used, so reconnecting or
disconnecting a pin between ticks is visible. Several old synthetic fixtures
disconnected their own output pin and only passed because the original emulator
retained a stale device through its alias. Those fixtures now keep their output
connected, and dedicated reconnection tests cover the corrected behavior.

## Observing device operations

Enable `RecordDeviceAccess` before execution. `DeviceAccesses` records the target
device identity, operation kind, property, slot/stack index, value, instruction
address and virtual tick. Host fixture setup is not recorded. Recording is off
by default to avoid accumulating traces in existing long-running tests.

For example, `RepeatedReadsAndWritesRemainOrderedAcrossYieldAndSleep` requires
two input reads, the original order of output writes, a boundary at `yield`, and
the specified delay at `sleep`. A final-state-only assertion would miss an
incorrectly removed or reordered operation.

Virtual ticks are deterministic and default to 0.5 seconds. `sleep` suspends
execution without sleeping the test process. These checks establish explicit
fixture behavior, not a claim that every optimized program must execute the
same number of instructions or finish on an identical tick.

## Interpreter scope and limitations

This is a test interpreter for the compiler's fixtures, not a complete or
game-certified Stationeers emulator. Its support now includes the compiler's
unary and ternary operations, device/property/slot access, batch property/slot
reads and writes, device memory, reagent reads/mapping, sleep and halt. Existing
integer coercion for bitwise binary operands now truncates toward zero, matching
the compiler's literal conversion; negative fractional operands have a regression
case. This does not certify wide bitwise masking or every numerical edge case.

- Device properties remain permissive mock fields, not a Stationpedia access
  catalogue. Reagent mappings are fixture dictionaries, not a recipe engine.
- Registered devices and pins are treated as one data network; separate cable
  networks and channels are outside this suite.
- Raw fixtures use compiler-style numeric branch addresses. This is not a
  general IC10 assembler with label/define parsing.
- Random instructions and other unimplemented opcodes fail explicitly. No RNG
  conformance or complete instruction-set coverage is claimed.
- Empty batch cases specify Average=NaN, Sum=0, Minimum=positive infinity and
  Maximum=negative infinity. As with the other raw instruction cases, these are
  expected semantic checks, not recorded in-game observations.
- No frozen old/new compiler comparison or game-certified reference corpus is
  introduced here. Those can be separate additions if required.

The initial unary/ternary/named-batch probe had 30 failures out of 31 on the
unchanged upstream interpreter. The interpreter fixes make those cases pass;
the compiler cases then exercise generated assembly through that interpreter.
Passing this suite increases regression coverage but is not proof of correctness
for every possible program or a substitute for independent conformance evidence.
