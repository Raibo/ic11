using System.Globalization;

namespace ic11.Tests;

using Operations;

public sealed class Emulator
{
    private readonly int _cyclesPerTick = 128;

    public List<Device?> Devices = null!;
    public Dictionary<int, Device> BatchDevices = null!; // devices that are used in batch operations
    public Dictionary<string, Device> DeviceAliases = null!;
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
        sb.AppendLine($"Clock: {Clock}");
        sb.AppendLine($"Program Counter: {ProgramCounter}: >> {_program[ProgramCounter]}");
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

    public Device GetOrCreateDeviceById(string device)
    {
        var id = (int)InferValue(device);
        if (BatchDevices.TryGetValue(id, out var byId))
        {
            return byId;
        }

        var newDevice = new Device();
        BatchDevices[id] = newDevice;
        return newDevice;
    }

    public Device? InferDevice(string device)
    {
        if (DeviceAliases.TryGetValue(device, out var d))
            return d;

        var register = device[1..];
        if (RegisterAliases.TryGetValue(register, out var index))
        {
            var deviceIndex = (int)Registers[index];

            if (deviceIndex < 0 || deviceIndex >= Devices.Count)
                throw GenerateEmulatorException($"Device index out of range: {deviceIndex}");

            return Devices[deviceIndex];
        }

        throw GenerateEmulatorException($"Unknown device: {device}");
    }
}