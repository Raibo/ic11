namespace ic11.ControlFlow.Context;
public class UserDefinedVariable
{
    public string Name;
    public Variable Variable;
    public int DeclaredIndex;
    public int LastReassignedIndex = -1;
    public int LastReferencedIndex = -1;

    public UserDefinedVariable(string name, Variable variable, int declaredIndex)
    {
        Name = name;
        Variable = variable;
        DeclaredIndex = declaredIndex;
    }

    public override string ToString() => $"{{{Name}, declared {DeclaredIndex}, last referenced {LastReferencedIndex}, last reassigned {LastReassignedIndex}}}";
}
