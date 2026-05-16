namespace ic11.ControlFlow.NodeInterfaces;
public interface IExpressionContainer
{
    public IEnumerable<INodeExpression> Expressions { get; }
}
