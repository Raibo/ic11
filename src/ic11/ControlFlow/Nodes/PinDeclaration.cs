using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class PinDeclaration : Node, IStatement
{
    public string Name;
    public string Device;

    public PinDeclaration(string name, string device)
    {
        Name = name;
        Device = device;
    }
}
