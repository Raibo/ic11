using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;
using Scope = ic11.ControlFlow.Context.Scope;

namespace ic11.ControlFlow.TreeProcessing;
public class ScopeVisitor
{
    private Scope _currentScope = new();

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
        node.IndexInScope = _currentScope.CurrentNodeOrder++;

        if (statement is If ifStatement)
        {
            Visit(ifStatement);
            return;
        }

        if (statement is IStatementsContainer st)
        {
            _currentScope = _currentScope.CreateChildScope(statement is MethodDeclaration md ? md : null);
            VisitStatementList(st.Statements);
            _currentScope = _currentScope.Parent!;
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
            _currentScope = _currentScope.Parent!;
        }

        node.CurrentStatementsContainer = IfStatementsContainer.Else;

        if (node.Statements.Any())
        {
            _currentScope = _currentScope.CreateChildScope();
            VisitStatementList(node.Statements);
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
        node.IndexInScope = _currentScope.CurrentNodeOrder++;
    }
}
