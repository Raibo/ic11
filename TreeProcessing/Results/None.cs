namespace ic11.TreeProcessing.Results;

public class None : IValue
{
    public None()
    {
    }

    public string Render() => "<NoneValue>";

    public static readonly None Instance = new();
}