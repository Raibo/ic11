using System.Reflection;

namespace ic11.Tests.Operations;

public abstract class Operation
{
    private static readonly Dictionary<string, Type> Operations;
    private static readonly Dictionary<string, int> ArgumentCounts;

    static Operation()
    {
        Operations = new();
        ArgumentCounts = new();
        // list subclasses of Operation
        foreach (var type in typeof(Operation).Assembly.GetTypes())
        {
            if (type.IsSubclassOf(typeof(Operation)) && !type.IsAbstract)
            {
                var opcodes = type.GetField("Opcodes", BindingFlags.Public | BindingFlags.Static);
                var values = (string[])opcodes!.GetValue(null)!;
                var argumentCount = type.GetField("ArgumentCount", BindingFlags.Public | BindingFlags.Static);
                var count = argumentCount != null ? (int)argumentCount.GetValue(null)! : 0;

                foreach (var value in values)
                {
                    Operations[value] = type;
                    ArgumentCounts[value] = count;
                }
            }
        }
    }

    public static Operation BuildOperation(string opcode, string[] args)
    {
        var type = Operations[opcode];
        var argumentCount = ArgumentCounts[opcode];

        if (args.Length != argumentCount)
        {
            throw new ArgumentException(
                $"Operation '{opcode}' requires exactly {argumentCount} arguments, but received {args.Length}.");
        }

        var operation = (Operation)Activator.CreateInstance(type)!;
        return operation.Build(opcode, args);
    }


    protected string[] Args = null!;
    protected string Opcode = null!;

    // ReSharper disable once MemberCanBePrivate.Global
    protected Operation Build(string opcode, string[] args)
    {
        Args = args;
        Opcode = opcode;

        return this;
    }


    public abstract void Execute(Emulator emulator);

    public override string ToString()
    {
        return $"{Opcode} {string.Join(" ", Args)}";
    }
}