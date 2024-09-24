namespace ic11.TreeProcessing.Results;

public class UserValue : IValue
{
    public UserValue(string name, IValue value)
    {
        Name = name;
        Value = value;
    }

    public string Name;
    public IValue Value;

    public override string ToString() => "VAR " + Name;
    public string Render() => Value.Render();
}