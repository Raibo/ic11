using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class ReturnStatementsVisitor : ControlFlowTreeVisitorBase<bool>
{
    protected override Type VisitorType => throw new NotImplementedException();

    public void Visit(Root root)
    {
        var methodDeclarations = root.Statements.Where(s => s is MethodDeclaration md && md.ReturnType == DataHolders.MethodReturnType.Real);

        foreach (MethodDeclaration method in methodDeclarations)
        {
            Visit(method);
        }
    }

    private void Visit(MethodDeclaration node)
    {
        var lastStatement = node.Statements.Last();

        Visit((Node)lastStatement);
    }
}
