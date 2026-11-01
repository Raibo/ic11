using System.Globalization;
using ic11.Emulator.Operations;

namespace ic11.Emulator;

public sealed class Emulator
{
    private readonly int _cyclesPerTick = 128;

    public List<Device?> Devices = null!;
    // Registered network devices, keyed by ReferenceId; PrefabHash and NameHash are device properties.
    public Dictionary<int, Device> BatchDevices = null!;
    public Dictionary<string, Device> DeviceAliases = null!;
    public Dictionary<string, string> DeviceAliasAddresses = null!;
    public Dictionary<string, int> RegisterAliases = null!;
    private List<Operation> _program = null!;
    public double[] Registers = null!; // registers (r0 - r15)
    public int ProgramCounter;
    public double[] Stack = null!;
    public readonly int StackPointerRegister = 16;
    public readonly int ReturnAddressRegister = 17;
    public bool Yielding;
    public long Clock;
    public bool Stopped;
    public long Tick { get; private set; }
    public double TickSeconds { get; init; } = 0.5;
    public double TimeSeconds => Math.Max(0, Tick - 1) * TickSeconds;
    public double SleepingUntil { get; set; }
    public bool RecordDeviceAccess { get; set; }
    public List<DeviceAccess> DeviceAccesses { get; } = [];


    public Emulator()
    {
    }

    public Emulator(int cyclesPerTick)
    {
        _cyclesPerTick = cyclesPerTick;
    }

    public void Reset()
    {
        BatchDevices = new();
        Devices = [];
        DeviceAliases = new();
        DeviceAliasAddresses = new();

        for (int i = 0; i < 6; i++)
        {
            Devices.Add(new Device());
            DeviceAliases.Add($"d{i}", Devices[i]!);
        }

        Stack = new double[512];
        var db = new Device
        {
            Stack = Stack
        };
        Devices.Add(db);
        DeviceAliases.Add("db", db);

        Registers = new double[18]; // r0 - r17
        RegisterAliases = new();
        for (int i = 0; i < 18; i++)
        {
            Registers[i] = 0.0;
            RegisterAliases.Add($"r{i}", i);
        }

        RegisterAliases.Add("sp", StackPointerRegister);
        RegisterAliases.Add("ra", 17);

        ProgramCounter = 0;

        Clock = 0;
        Stopped = false;
        Yielding = false;
        Tick = 0;
        SleepingUntil = 0;
        DeviceAccesses.Clear();
    }


    public void LoadProgram(string[] programCode)
    {
        _program = new List<Operation>();
        foreach (var line in programCode)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#") || trimmedLine.StartsWith("//"))
            {
                _program.Add(new Noop());
            }
            else
            {
                var parts = trimmedLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                _program.Add(Operation.BuildOperation(parts[0], parts.Skip(1).ToArray()));
            }
        }

        // reset state 
        Reset();
    }

    public void Run(int ticks = 1)
    {
        if (Stopped)
            return;
        
        if (_program == null)
        {
            throw new Exception("Program not loaded");
        }

        while (ticks > 0)
        {
            Tick++;
            if (TimeSeconds < SleepingUntil)
            {
                ticks--;
                continue;
            }
            for (int i = 0; i < _cyclesPerTick; i++)
            {
                Clock++;

                if (ProgramCounter >= _program.Count)
                {
                    ProgramCounter = 0;
                    Stopped = true;
                    return; // end of program
                }

                var operation = _program[ProgramCounter];

                operation.Execute(this);

                if (Stopped)
                    return;

                if (Yielding)
                {
                    Yielding = false;
                    break;
                }
            }

            ticks--;
        }
    }

    public double InferValue(string value)
    {
        if (double.TryParse(value, CultureInfo.InvariantCulture, out var number))
            return number;

        if (RegisterAliases.TryGetValue(value, out var index))
            return Registers[index];

        throw GenerateEmulatorException($"Unknown value: {value}");
    }

    public void SetRegister(string register, double value)
    {
        if (RegisterAliases.TryGetValue(register, out var index))
        {
            Registers[index] = value;
        }
        else
        {
            throw GenerateEmulatorException($"Unknown register: {register}");
        }
    }

    public void PrintSummary()
    {
        Console.WriteLine(GetSummary());
    }

    public string GetSummary()
    {
        var sb = new System.Text.StringBuilder();
        var valuesPerLine = 10;
        
        sb.AppendLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        sb.AppendLine("Emulator Summary:");
        sb.AppendLine($"Program Length: {_program.Count}");
        sb.AppendLine($"Clock: {Clock}{(Stopped ? " (Stopped)" : "")}");
        sb.AppendLine($"Program Counter: {ProgramCounter}: >> {(ProgramCounter < _program.Count ? _program[ProgramCounter] : "Out of bounds")}");
        sb.AppendLine($"SP: {Registers[StackPointerRegister]}  RA: {Registers[ReturnAddressRegister]}");
        sb.AppendLine();

        for (int i = 0; i < 16; i++)
        {
            var rn = $"r{i}";
            sb.Append($"{rn,-3} ");
            var width = $"{Registers[i],-3}".Length;
            while (width > 3)
            {
                sb.Append(" ");
                width--;
            }
        }

        sb.AppendLine();

        for (int i = 0; i < 16; i++)
        {
            sb.Append($"{Registers[i].ToString(CultureInfo.InvariantCulture),-3} ");
        }

        sb.AppendLine();

        var maxNonZeroIndex = 0;
        for (int i = 0; i < Stack.Length; i++)
        {
            if (Stack[i] != 0)
                maxNonZeroIndex = i;
        }

        maxNonZeroIndex++;

        sb.AppendLine();
        sb.Append("Stack:");
        for (int i = 0; i < valuesPerLine; i++)
        {
            sb.Append($"{i,3} ");
        }

        sb.AppendLine();

        for (int i = 0; i < valuesPerLine + 1; i++)
        {
            sb.Append("----");
        }

        for (int i = 0; i <= maxNonZeroIndex && i < Stack.Length; i++)
        {
            if (i % valuesPerLine == 0)
            {
                sb.AppendLine();
                sb.Append($"{i,-3} | ");
            }

            if (i == (int)Registers[StackPointerRegister])
                sb.Append($"{Stack[i].ToString(CultureInfo.InvariantCulture),3}<");
            else
                sb.Append($"{Stack[i].ToString(CultureInfo.InvariantCulture),3} ");
        }
        
        sb.AppendLine();

        return sb.ToString();
    }


    public Exception GenerateEmulatorException(string unexpectedOperationType)
    {
        return new Exception(
            $"Unexpected operation type: {unexpectedOperationType}, program counter: {ProgramCounter}");
    }

    public Device GetDeviceById(string device)
    {
        var id = (int)InferValue(device);
        if (BatchDevices.TryGetValue(id, out var byId))
        {
            return byId;
        }

        throw GenerateEmulatorException($"Device ReferenceId is not registered: {id}");
    }

    public Device? InferDevice(string device)
    {
        if (DeviceAliasAddresses.TryGetValue(device, out var target))
            return InferDevice(target);
        if (device.StartsWith('d') && device != "db")
        {
            var indexText = device[1..];
            if (int.TryParse(indexText, out var pin) || RegisterAliases.ContainsKey(indexText))
            {
                if (!int.TryParse(indexText, out pin))
                    pin = (int)InferValue(indexText);
                if (pin < 0 || pin >= 6)
                    throw GenerateEmulatorException($"Device pin out of range: {pin}");
                return Devices[pin];
            }
        }
        if (DeviceAliases.TryGetValue(device, out var d))
            return d;

        return GetDeviceById(device);
    }

    public Device RequireDevice(string address) => InferDevice(address)
        ?? throw GenerateEmulatorException($"Device pin is not connected: {address}");

    public Device RegisterDevice(int referenceId, int prefabHash, int nameHash = 0, Device? device = null)
    {
        device ??= new Device();
        device.SetProperty("ReferenceId", referenceId);
        device.SetProperty("PrefabHash", prefabHash);
        device.SetProperty("NameHash", nameHash);
        BatchDevices.Add(referenceId, device);
        return device;
    }

    public IEnumerable<Device> SelectDevices(int prefabHash, int? nameHash = null) =>
        Devices.OfType<Device>().Concat(BatchDevices.Values).Distinct()
            .Where(device => device.GeneralProperties.TryGetValue("PrefabHash", out var hash) && hash == prefabHash)
            .Where(device => nameHash is null || device.GetProperty("NameHash") == nameHash);

    public void RecordAccess(Device device, DeviceAccessKind kind, double value = 0, string? property = null, int? index = null)
    {
        if (RecordDeviceAccess)
            DeviceAccesses.Add(new DeviceAccess(Tick, ProgramCounter, device, kind, property, index, value));
    }

    public double ReadProperty(Device device, string property)
    {
        var value = device.GetProperty(property);
        RecordAccess(device, DeviceAccessKind.ReadProperty, value, property);
        return value;
    }

    public void WriteProperty(Device device, string property, double value)
    {
        device.SetProperty(property, value);
        RecordAccess(device, DeviceAccessKind.WriteProperty, value, property);
    }

    public double ReadSlot(Device device, int slot, string property)
    {
        var value = device.GetSlotProperty(slot, property);
        RecordAccess(device, DeviceAccessKind.ReadSlot, value, property, slot);
        return value;
    }

    public void WriteSlot(Device device, int slot, string property, double value)
    {
        device.SetSlotProperty(slot, property, value);
        RecordAccess(device, DeviceAccessKind.WriteSlot, value, property, slot);
    }
}
