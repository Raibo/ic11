namespace ic11.ControlFlow.Context;
public class UserDefinedConstant
{
    public string Name;
    public decimal CtKnownValue;
    public int DeclaredIndex;
    public int LastReferencedIndex = -1;

    public UserDefinedConstant(string name, decimal ctKnownValue, int declaredIndex)
    {
        Name = name;
        CtKnownValue = ctKnownValue;
        DeclaredIndex = declaredIndex;
    }

    public override string ToString() =>
        $"{{ {Name}, declared {DeclaredIndex}, last referenced {LastReferencedIndex}, value {CtKnownValue}";
}
