using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class RegisterVisitor
{
    public void Visit(Root node)
    {
        foreach (Node statement in node.Statements)
            VisitNode(statement);
    }

    private object? VisitNode(Node node)
    {
        if (node is IExpressionContainer ec)
        {
            foreach (Node item in ec.Expressions)
                VisitNode(item);
        }

        if (node is IExpression ex && ex.Variable is not null && ex.Variable.Register is null)
            ex.Variable.Register = node.Scope!.GetAvailableRegister(node.IndexInScope);

        if (node is VariableDeclaration d && d.Variable is not null && d.Variable.Register is null)
            d.Variable.Register = node.Scope!.GetAvailableRegister(node.IndexInScope);

        if (node is IStatementsContainer sc)
        {
            foreach (Node item in sc.Statements)
                VisitNode(item);
        }

        return null;
    }
}
