using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class MethodsVisitor : ControlFlowTreeVisitorBase<bool>
{
    protected override Type VisitorType => typeof(MethodsVisitor);

    private readonly FlowContext _flowContext;
    private MethodDeclaration _currentMethod;

    public MethodsVisitor(FlowContext flowContext)
    {
        _flowContext = flowContext;
        AllowMethodSkip = true;
        SkippedReturnValue = false;
    }

    public void Visit(Root root)
    {
        var methodDeclarations = root.Statements.OfType<MethodDeclaration>();

        foreach (MethodDeclaration method in methodDeclarations)
            Visit(method);

        if (!_flowContext.DeclaredMethods.ContainsKey("Main"))
            throw new Exception($"Missing method 'void Main()'");
    }

    private bool Visit(Return node)
    {
        if (_currentMethod.ReturnType == DataHolders.MethodReturnType.Void && node.HasValue)
            throw new Exception($"Unexpected return value in a void method");

        if (_currentMethod.ReturnType != DataHolders.MethodReturnType.Void && !node.HasValue)
            throw new Exception($"Return value expected");

        return true;
    }

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
        if (_flowContext.DeclaredMethods.ContainsKey(node.Name))
            throw new Exception($"Method '{node.Name}' already exists");

        if (node.Name == "Main" && node.Parameters.Any())
            throw new Exception($"Method '{node.Name}' cannot have parameters");

        if (node.Name == "Main" && node.ReturnType != DataHolders.MethodReturnType.Void)
            throw new Exception($"Method '{node.Name}' cannot return value");

        _flowContext.DeclaredMethods[node.Name] = node;
        _currentMethod = node;

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

    private bool Visit(For node)
    {
        ContainReturn(node.Statements);

        return false;
    }

    private bool Visit(ArrayDeclaration node)
    {
        _currentMethod.ContainsArrays = true;

        return false;
    }
}
