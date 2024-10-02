using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.Context;
public class UserDefinedArray
{
    public string Name;
    public ArrayDeclaration Array;
    public int DeclaredIndex;
    public int LastReassignedIndex = -1;
    public int LastReferencedIndex = -1;

    public UserDefinedArray(string name, ArrayDeclaration array, int declaredIndex)
    {
        Name = name;
        Array = array;
        DeclaredIndex = declaredIndex;
    }

    public override string ToString() =>
        $"{{ arr {Name}, declared {DeclaredIndex}, last referenced {LastReferencedIndex}, " +
        $"last reassigned {LastReassignedIndex} }}";
}
