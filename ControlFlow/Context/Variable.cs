namespace ic11.ControlFlow.Context;
public class Variable
{
    public readonly int Id = _staticId++;

    private static int _staticId = 0;

    public int DeclareIndex;
    public int LastUseIndex = -1;
    public int LastReassignedIndex = -1;

    public string Register;
}
