using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class For : Node, IStatement, IStatementsContainer, IExpressionContainer
{
#pragma warning disable CS8618
    /// <remarks> Promise to fill in <see cref="nameof(ControlFlowBuilderVisitor)"/>. </remarks>

    public INodeExpression Expression;

#pragma warning restore CS8618

    public List<IStatement> Statements { get; set; } = new();

    public bool HasStatement1;
    public bool HasStatement2;

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return Expression;
        }
    }
}
