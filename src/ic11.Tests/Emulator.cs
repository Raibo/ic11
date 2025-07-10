namespace ic11.Tests;

using ic11.Tests.Operations;

public sealed class Emulator
{
    private const int CyclesPerTick = 128;

    public List<Device> Devices = null!;
    public Dictionary<int, Device> BatchDevices = null!; // devices that are used in batch operations
    public Dictionary<string, Device> DeviceAliases = null!;
    public Dictionary<string, int> RegisterAliases = null!;
    private List<Operation> _program = null!;
    public double[] Registers = null!; // registers (r0 - r15)
    public int ProgramCounter;
    public double[] Stack = null!;
    public readonly int StackPointerRegister = 16;

    public void Reset()
    {
        BatchDevices = new();
        Devices = [];
        DeviceAliases = new();

        for (int i = 0; i < 6; i++)
        {
            Devices.Add(new Device());
            DeviceAliases.Add($"d{i}", Devices[i]);
        }

        var db = new Device();
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
        Stack = new double[512];
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
        if (_program == null)
        {
            throw new Exception("Program not loaded");
        }

        // run shit
        for (int i = 0; i < CyclesPerTick; i++)
        {
            var operation = _program[ProgramCounter];

            operation.Execute(this);
        }
    }

    public double InferValue(string value)
    {
        if (double.TryParse(value, out var number))
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
        Stack[333] = 123.456; // just to show that stack works

        var valuesPerLine = 10;

        Console.WriteLine("Emulator Summary:");
        Console.WriteLine($"Program Counter: {ProgramCounter}");
        Console.WriteLine($"Stack Pointer (sp): {Registers[StackPointerRegister]}");

        Console.WriteLine("Registers:");
        for (int i = 0; i < Registers.Length; i++)
        {
            var rn = $"r{i}";
            Console.Write($"{rn,-3} ");
        }

        Console.WriteLine();

        for (int i = 0; i < Registers.Length; i++)
        {
            Console.Write($"{Registers[i],-3} ");
        }

        Console.WriteLine();

        Console.WriteLine("Stack:");
        var maxNonZeroIndex = 0;
        for (int i = 0; i < Stack.Length; i++)
        {
            if (Stack[i] != 0)
                maxNonZeroIndex = i;
        }

        Console.Write($"     ");
        for (int i = 0; i < valuesPerLine; i++)
        {
            Console.Write($"{i,3} ");
        }

        Console.WriteLine();

        for (int i = 0; i < valuesPerLine + 1; i++)
        {
            Console.Write($"----");
        }

        for (int i = 0; i <= maxNonZeroIndex; i++)
        {
            if (i % valuesPerLine == 0)
            {
                Console.WriteLine();
                Console.Write($"{i,-3}| ");
            }

            Console.Write($"{Stack[i],3} ");
        }
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

    public Device InferDevice(string device)
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