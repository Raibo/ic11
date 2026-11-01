namespace ic11.Emulator;

public enum DeviceAccessKind
{
    ReadProperty,
    WriteProperty,
    ReadSlot,
    WriteSlot,
    ReadStack,
    WriteStack,
    ClearStack,
    ReadReagent,
    MapReagent,
}

/// <summary>An observable device operation executed by the program, excluding test setup.</summary>
public sealed record DeviceAccess(
    long Tick, int ProgramCounter, Device Device, DeviceAccessKind Kind,
    string? Property, int? Index, double Value);
