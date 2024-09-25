namespace ic11.TreeProcessing.Context;
public class DeclaredMethod
{
    public string Name;
    public bool ReturnsValue;
    public int ParamCount;

    public DeclaredMethod(string name, bool returnsValue, int paramCount)
    {
        Name = name;
        ReturnsValue = returnsValue;
        ParamCount = paramCount;
    }
}
