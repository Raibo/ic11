using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class VariableCyclesAdjVisitor : ControlFlowTreeVisitorBase<object?>
{
    protected override Type VisitorType => typeof(VariableCyclesAdjVisitor);

    public VariableCyclesAdjVisitor()
    {
        AllowMethodSkip = true;
    }

    public void VisitRoot(Root root)
    {
        foreach (var item in root.Statements)
            Visit(item);
    }

    protected override object? Visit(While node)
    {
        var cycleStartIndex = node.Expression.FirstIndexInTree;
        var cycleFinishIndex = LastStatementIndex(node);

        var relevantVariables = node.Scope!.Variables
            .Where(v => v.DeclareIndex < cycleStartIndex)
            .Where(v => cycleStartIndex <= v.LastReferencedIndex && v.LastReferencedIndex <= cycleFinishIndex)
            .ToList();

        foreach (var variable in relevantVariables)
            variable.LastReferencedIndex = Math.Max(cycleFinishIndex + 1, variable.LastReferencedIndex);

        foreach (INode item in node.Statements)
            Visit(item);

        return null;
    }

    protected override object? Visit(For node)
    {
        var cycleStartIndex = node.HasStatement1
            ? node.Statements.First().IndexInScope
            : node.Expression.FirstIndexInTree;

        var cycleFinishIndex = LastStatementIndex(node);

        var relevantVariables = node.Scope!.Variables
            .Where(v => v.DeclareIndex < cycleStartIndex)
            .Where(v => cycleStartIndex <= v.LastReferencedIndex && v.LastReferencedIndex <= cycleFinishIndex)
            .ToList();

        if (node.HasStatement1 && node.Statements.First() is VariableDeclaration dec)
            relevantVariables.Add(dec.Variable!);

        foreach (var variable in relevantVariables)
            variable.LastReferencedIndex = Math.Max(cycleFinishIndex + 1, variable.LastReferencedIndex);

        foreach (INode item in node.Statements)
            Visit(item);

        return null;
    }

    private static int LastStatementIndex(IStatement node)
    {
        return GetLastIndex(node);

        int GetLastIndex(IStatement statement)
        {
            if (statement is If ifStatement)
            {
                ifStatement.CurrentStatementsContainer = ifStatement.ElseStatements.Any()
                    ? DataHolders.IfStatementsContainer.Else
                    : DataHolders.IfStatementsContainer.If;
            }

            if (statement is not IStatementsContainer sc || !sc.Statements.Any())
                return statement.IndexInScope;

            return sc.Statements.Max(GetLastIndex);
        }
    }
}
