using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;
using Scope = ic11.ControlFlow.Context.Scope;

namespace ic11.ControlFlow.TreeProcessing;
public class ScopeVisitor
{
    private Scope _currentScope = new();
    private readonly FlowContext _flowContext;

    public ScopeVisitor(FlowContext flowContext)
    {
        _flowContext = flowContext;
    }

    public object? Visit(Root node)
    {
        node.Scope = _currentScope;

        VisitStatementList(((IStatementsContainer)node).Statements);

        return default!;
    }

    private void VisitStatementList(IEnumerable<IStatement> list)
    {
        foreach (var item in list)
            VisitStatement(item);
    }

    private void VisitStatement(IStatement statement)
    {
        if (statement is IExpressionContainer ec)
            foreach (var item in ec.Expressions)
                VisitExpression(item);

        var node = (Node)statement;
        node.Scope = _currentScope;
        node.SetIndex(ref _currentScope.CurrentNodeOrder);

        if (statement is If ifStatement)
        {
            Visit(ifStatement);
            return;
        }

        if (statement is IStatementsContainer st)
        {
            var md = statement as MethodDeclaration;

            _currentScope = _currentScope.CreateChildScope(md);

            if (md is not null)
            {
                md.InnerScope = _currentScope;
                AddParameterVariables(md);
            }

            VisitStatementList(st.Statements);
            _currentScope.Parent!.CurrentNodeOrder = _currentScope.CurrentNodeOrder;
            _currentScope = _currentScope.Parent!;
        }
    }

    private void AddParameterVariables(MethodDeclaration node)
    {
        foreach (var parameter in node.Parameters)
        {
            var variable = _currentScope!.ClaimNewVariable(-1);
            node.ParameterVariables.Add(variable);

            var newUserDefinedVariable = new UserDefinedVariable(parameter, variable!, -1, false);

            _currentScope.AddUserVariable(newUserDefinedVariable);
            _flowContext.AllUserDefinedVariables.Add(newUserDefinedVariable);
        }
    }

    protected object? Visit(If node)
    {
        VisitExpression(node.Expression);

        node.CurrentStatementsContainer = IfStatementsContainer.If;

        if (node.Statements.Any())
        {
            _currentScope = _currentScope.CreateChildScope();
            VisitStatementList(node.Statements);
            _currentScope.Parent!.CurrentNodeOrder = _currentScope.CurrentNodeOrder;
            _currentScope = _currentScope.Parent!;
        }

        node.CurrentStatementsContainer = IfStatementsContainer.Else;

        if (node.Statements.Any())
        {
            _currentScope = _currentScope.CreateChildScope();
            VisitStatementList(node.Statements);
            _currentScope.Parent!.CurrentNodeOrder = _currentScope.CurrentNodeOrder;
            _currentScope = _currentScope.Parent!;

        }

        return default!;
    }

    private void VisitExpression(IExpression expression)
    {
        if (expression is IExpressionContainer innerContainer)
        {
            foreach (var item in innerContainer.Expressions)
                VisitExpression(item);
        }

        var node = (Node)expression;
        node.Scope = _currentScope;
        node.SetIndex(ref _currentScope.CurrentNodeOrder);
    }
}
