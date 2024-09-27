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

    private void Visit(MethodDeclaration node)
    {
        for (int i = 0; i < node.Statements.Count; i++)
        {
            var statement = (Node)node.Statements[i];
            var guaranteedReturn = Visit(statement);

            if (guaranteedReturn && !statement.Equals(node.Statements.Last()))
                Console.WriteLine($"Unreachable code in method {node.Name}");

            if (guaranteedReturn)
                return;
        }

        if (node.ReturnType == DataHolders.MethodReturnType.Real)
            Console.WriteLine($"Not all code paths return a value in method {node.Name}");
    }

    private bool Visit(Return node) => true;

    private bool Visit(If node)
    {
        bool ifGuaranteedReturn = false;

        foreach (Node item in node.IfStatements)
        {
            var guaranteedReturn = Visit(item);

            if (guaranteedReturn && !item.Equals(node.IfStatements.Last()))
                Console.WriteLine($"Unreachable code in 'if'");

            if (guaranteedReturn)
            {
                ifGuaranteedReturn = true;
                break;
            }
        }

        bool elseGuaranteedReturn = false;

        foreach (Node item in node.ElseStatements)
        {
            var guaranteedReturn = Visit(item);

            if (guaranteedReturn && !item.Equals(node.ElseStatements.Last()))
                Console.WriteLine($"Unreachable code in 'else'");

            if (guaranteedReturn)
            {
                elseGuaranteedReturn = true;
                break;
            }
        }

        return ifGuaranteedReturn && elseGuaranteedReturn;
    }

    private bool Visit(While node)
    {
        foreach (Node item in node.Statements)
        {
            var guaranteedReturn = Visit(item);

            if (guaranteedReturn && !item.Equals(node.Statements.Last()))
                Console.WriteLine($"Unreachable code in 'while'");
        }

        return false;
    }
}
