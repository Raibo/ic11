using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class RegisterVisitor
{
    private HashSet<int> _registers = Enumerable.Range(0, 15).ToHashSet();

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
            ex.Variable.Register = GetAvailableRegister(node.Scope!, node.IndexInScope);

        if (node is VariableDeclaration d && d.Variable is not null && d.Variable.Register is null)
            d.Variable.Register = GetAvailableRegister(node.Scope!, node.IndexInScope);

        if (node is IStatementsContainer sc)
        {
            foreach (Node item in sc.Statements)
                VisitNode(item);
        }

        return null;
    }

    private string GetAvailableRegister(Scope scope, int index)
    {
        var usedRegisters = scope.Variables
            .Where(v => v.DeclareIndex < index)
            .Where(v => v.LastUseIndex > index)
            .Select(v => v.Register)
            .ToHashSet();

        var availableRegisters = _registers
            .OrderBy(r => r)
            .Select(r => $"r{r}")
            .Except(usedRegisters);

        return availableRegisters.First();
    }
}
