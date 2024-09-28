using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class VariableVisitor : ControlFlowTreeVisitorBase<Variable?>
{
    protected override Type VisitorType => typeof(VariableVisitor);
    private Root _root;

    private HashSet<Type> _preciselyTreatedNodes = new()
    {
        typeof(VariableDeclaration),
        typeof(VariableAssignment),
        typeof(VariableAccess),
        typeof(PinDeclaration),
        typeof(If),
    };

    public VariableVisitor()
    {
        AllowMethodSkip = true;
    }

    public void Visit(Root node)
    {
        _root = node;

        foreach (Node statement in node.Statements)
            VisitNode(statement);
    }

    private Variable? VisitNode(Node node)
    {
        if (_preciselyTreatedNodes.Contains(node.GetType()))
            return Visit(node);

        Variable? variable = null;

        if (node is IExpressionContainer ec)
        {
            foreach (Node item in ec.Expressions)
            {
                var innerVariable = VisitNode(item);

                if (innerVariable is not null)
                    innerVariable.LastUseIndex = node.IndexInScope;
            }
        }

        if (node is IExpression ex)
        {
            ex.Variable = node.Scope!.ClaimNewVariable();

            if (ex.Variable is not null)
                ex.Variable.DeclareIndex = node.IndexInScope;

            variable = ex.Variable;
        }

        if (node is IStatementsContainer sc)
        {
            foreach (Node item in sc.Statements)
                VisitNode(item);
        }

        return variable;
    }

    protected Variable? Visit(VariableDeclaration node)
    {
        var exprVar = VisitNode((Node)node.Expression);

        if (exprVar is not null)
            exprVar.LastUseIndex = node.IndexInScope;

        var scope = node.Scope!;

        node.Variable = node.Scope!.ClaimNewVariable();
        node.Variable.DeclareIndex = node.IndexInScope;

        if (scope.UserDefinedVariables.ContainsKey(node.Name))
            throw new Exception($"Variable already exists");

        scope.UserDefinedVariables[node.Name] = node.Variable!;

        return null;
    }

    private Variable? Visit(VariableAssignment node)
    {
        if (!node.Scope!.UserDefinedVariables.TryGetValue(node.Name, out var targetVariable))
            throw new Exception($"Variable {node.Name} is not defined");

        targetVariable.LastReassignedIndex = node.IndexInScope;
        node.Variable = targetVariable;

        var expressionVariable = VisitNode((Node)node.Expression);

        if (expressionVariable is not null)
            expressionVariable.LastUseIndex = node.IndexInScope;

        return null;
    }

    private Variable Visit(VariableAccess node)
    {
        if (!node.Scope!.UserDefinedVariables.TryGetValue(node.Name, out var variable))
            throw new Exception($"Variable {node.Name} is not defined");

        node.Variable = variable;
        variable.LastUseIndex = node.IndexInScope;

        return variable;
    }

    private Variable? Visit(PinDeclaration node)
    {
        if (node is PinDeclaration pin)
        {
            if (_root.DevicePinMap.Values.Contains(pin.Device))
                throw new Exception($"Pin {pin.Device} already has ");

            _root.DevicePinMap[pin.Name] = pin.Device;
        }

        return null;
    }
}
