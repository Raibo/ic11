using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MethodCall : Node, IStatement, IExpression, IExpressionContainer
{
    public string Name;
    public MethodDeclaration? Method;
    public List<IExpression> ArgumentExpressions;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;
    public HashSet<string> RegistersToPush;

    public override int IndexSize => 2;

    public MethodCall(string name, List<IExpression> argumentExpressions)
    {
        Name = name;
        ArgumentExpressions = argumentExpressions;

        foreach (var item in argumentExpressions)
            ((Node)item).Parent = this;
    }

    public IEnumerable<IExpression> Expressions => ArgumentExpressions;
}
