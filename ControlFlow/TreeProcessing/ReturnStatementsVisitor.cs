using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class ReturnStatementsVisitor : ControlFlowTreeVisitorBase<bool>
{
    protected override Type VisitorType => typeof(ReturnStatementsVisitor);

    public ReturnStatementsVisitor()
    {
        AllowMethodSkip = true;
        SkippedReturnValue = false;
    }

    public void Visit(Root root)
    {
        var methodDeclarations = root.Statements.Where(s => s is MethodDeclaration md);

        foreach (MethodDeclaration method in methodDeclarations)
        {
            Visit(method);
        }
    }
    private bool Visit(Return node) => true;

    private bool ContainReturn(IEnumerable<IStatement> statements)
    {
        bool foundReturnStatement = false;

        foreach (Node statement in statements)
        {
            if (foundReturnStatement)
            {
                statement.IsUnreachableCode = true;
                continue;
            }

            var guaranteedReturn = Visit(statement);

            if (guaranteedReturn)
                foundReturnStatement = true;
        }

        return foundReturnStatement;
    }

    private void Visit(MethodDeclaration node)
    {
        bool containReturn = ContainReturn(node.Statements);

        if (!containReturn && node.ReturnType == DataHolders.MethodReturnType.Real)
            node.NotAllPathsReturnValue = true;
    }

    private bool Visit(If node)
    {
        bool ifGuaranteedReturn = ContainReturn(node.IfStatements);
        bool elseGuaranteedReturn = ContainReturn(node.ElseStatements);

        return ifGuaranteedReturn && elseGuaranteedReturn;
    }

    private bool Visit(While node)
    {
        ContainReturn(node.Statements);

        return false;
    }
}
