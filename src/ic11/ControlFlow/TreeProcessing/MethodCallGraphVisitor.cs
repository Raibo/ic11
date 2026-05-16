using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class MethodCallGraphVisitor : ControlFlowTreeVisitorBase<Variable?>
{
    protected override Type VisitorType => typeof(MethodCallGraphVisitor);
    private readonly FlowContext _flowContext;

    private MethodDeclaration _currentMethod;

    private HashSet<Type> _preciselyTreatedNodes = new()
    {
        typeof(MethodCall),
        typeof(MethodDeclaration),
    };

    public MethodCallGraphVisitor(FlowContext flowContext)
    {
        AllowMethodSkip = true;
        _flowContext = flowContext;
        _currentMethod = _flowContext.DeclaredMethods["Main"];

        return;
    }

    public void VisitRoot(Root root)
    {
        Visit((IStatementsContainer)root);
    }

    private void Visit(IStatementsContainer node)
    {
        if (node is MethodDeclaration md)
            _currentMethod = md;

        IEnumerable<IStatement> statements;

        if (node is If ifn)
        {
            ifn.CurrentStatementsContainer = DataHolders.IfStatementsContainer.If;
            statements = ifn.Statements;

            ifn.CurrentStatementsContainer = DataHolders.IfStatementsContainer.Else;
            statements = statements.Concat(ifn.Statements);
        }
        else
        {
            statements = node.Statements;
        }

        foreach (var item in statements)
            Visit(item);
    }

    private void Visit(IExpressionContainer node)
    {
        foreach (var item in node.Expressions)
            Visit(item);
    }

    private void Visit(INode node)
    {
        if (node is IExpressionContainer ic)
            Visit(ic);

        if (node is IStatementsContainer sc)
            Visit(sc);

        if (node is MethodCall mc)
            Visit(mc);
    }

    private Variable? Visit(MethodCall node)
    {
        _currentMethod.InvokedMethods.Add(node.Method!);
        node.Method!.InvokedFrom.Add(_currentMethod);
        _currentMethod.MethodCalls.Add(node);

        return null;
    }
}
