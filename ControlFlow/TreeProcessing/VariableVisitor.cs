using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class VariableVisitor : ControlFlowTreeVisitorBase<Variable?>
{
    protected override Type VisitorType => typeof(VariableVisitor);
    private readonly FlowContext _flowContext;
    private Root _root;

    private HashSet<Type> _preciselyTreatedNodes = new()
    {
        typeof(VariableDeclaration),
        typeof(VariableAssignment),
        typeof(VariableAccess),
        typeof(PinDeclaration),
        typeof(If),
    };

    public VariableVisitor(FlowContext flowContext)
    {
        AllowMethodSkip = true;
        _flowContext = flowContext;
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
                    innerVariable.LastReferencedIndex = node.IndexInScope;
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
            exprVar.LastReferencedIndex = node.IndexInScope;

        var scope = node.Scope!;

        node.Variable = node.Scope!.ClaimNewVariable();
        node.Variable.DeclareIndex = node.IndexInScope;

        if (scope.UserDefinedVariables.ContainsKey(node.Name))
            throw new Exception($"Variable already exists");

        var newUserDefinedVariable = new UserDefinedVariable(node.Name, node.Variable!, node.IndexInScope);
        scope.UserDefinedVariables[node.Name] = newUserDefinedVariable;
        _flowContext.AllUserDefinedVariables.Add(newUserDefinedVariable);

        return null;
    }

    private Variable? Visit(VariableAssignment node)
    {
        if (!node.Scope!.UserDefinedVariables.TryGetValue(node.Name, out var targetVariable))
            throw new Exception($"Variable {node.Name} is not defined");

        targetVariable.LastReassignedIndex = node.IndexInScope;
        node.Variable = targetVariable.Variable;

        var expressionVariable = VisitNode((Node)node.Expression);

        if (expressionVariable is not null)
            expressionVariable.LastReferencedIndex = node.IndexInScope;

        return null;
    }

    private Variable Visit(VariableAccess node)
    {
        if (!node.Scope!.UserDefinedVariables.TryGetValue(node.Name, out var userDefinedVariable))
            throw new Exception($"Variable {node.Name} is not defined");

        node.Variable = userDefinedVariable.Variable;
        userDefinedVariable.LastReferencedIndex = node.IndexInScope;
        userDefinedVariable.Variable.LastReferencedIndex = node.IndexInScope;

        return userDefinedVariable.Variable;
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
