namespace ic11.Tests;

public class AnyValue
{
    private readonly int? _registerNumber;
    private readonly double? _number;
    private readonly Emulator _emulator;

    public AnyValue(string value, Emulator context)
    {
        _emulator = context;
        if (double.TryParse(value, out var numberValue))
        {
            _number = numberValue;
            _registerNumber = null;
        }
        else
        {
            _number = null;
            _registerNumber = context.RegisterAliases[value];
        }
    }

    public double Value =>
        _number ?? _emulator.Registers[_registerNumber!.Value];
}

