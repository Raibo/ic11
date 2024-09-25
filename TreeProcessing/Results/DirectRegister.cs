namespace ic11.TreeProcessing.Results;

public class DirectRegister : IValue
{
    public DirectRegister(string register)
    {
        Register = register;
    }

    public string Register;

    public override string ToString() => $"Direct register access [{Register}]";
    public string Render() => Register;
}