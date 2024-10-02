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
            Visit((Node)item);
    }

    protected override object? Visit(While node)
    {
        var cycleStartIndex = node.Expression.FirstIndexInTree;
        var cycleFinishIndex = LastStatementIndex(node);

        var relevantVariables = node.Scope!.Variables
            .Where(v => v.DeclareIndex < cycleStartIndex)
            .Where(v => v.LastReferencedIndex <= cycleFinishIndex)
            .Where(v => v.LastReferencedIndex >= cycleStartIndex)
            .ToList();

        foreach (var variable in relevantVariables)
        {
            variable.LastReferencedIndex = Math.Max(cycleFinishIndex, variable.LastReferencedIndex);
        }

        foreach (Node item in node.Statements)
            Visit(item);

        return null;
    }

    private static int LastStatementIndex(While node) => ((Node)node.Statements.Last()).IndexInScope;
}
