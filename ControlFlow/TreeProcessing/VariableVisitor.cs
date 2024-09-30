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
        typeof(ConstantDeclaration),
        typeof(VariableAssignment),
        typeof(UserDefinedValueAccess),
        typeof(PinDeclaration),
        typeof(If),
        typeof(MethodDeclaration),
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
            if (ex.CtKnownValue is null)
                ex.Variable = node.Scope!.ClaimNewVariable();

            if (ex.Variable is not null)
                ex.Variable.DeclareIndex = node.IndexInScope;

            if (IsVoidCallAsExpression(node))
                throw new Exception($"Void method used as an expression");

            variable = ex.Variable;
        }

        if (node is IStatementsContainer sc)
        {
            foreach (Node item in sc.Statements)
                VisitNode(item);
        }

        return variable;

        bool IsVoidCallAsExpression(Node node)
        {
            if (node is not MethodCall mc)
                return false;

            var isCalledMethodVoid = _flowContext.DeclaredMethods[mc.Name].ReturnType == DataHolders.MethodReturnType.Void;
            var isInExpressionsList = node.Parent is IExpressionContainer ec && ec.Expressions.Any(x => node.Equals(x));

            return isCalledMethodVoid && isInExpressionsList;
        }
    }

    protected Variable? Visit(VariableDeclaration node)
    {
        var exprVar = VisitNode((Node)node.Expression);

        if (exprVar is not null)
            exprVar.LastReferencedIndex = node.IndexInScope;

        var scope = node.Scope!;

        node.Variable = node.Scope!.ClaimNewVariable();
        node.Variable.DeclareIndex = node.IndexInScope;

        var newUserDefinedVariable = new UserDefinedVariable(node.Name, node.Variable!, node.IndexInScope, node.Expression.CtKnownValue.HasValue);

        scope.AddUserVariable(newUserDefinedVariable);
        _flowContext.AllUserDefinedVariables.Add(newUserDefinedVariable);

        return null;
    }

    protected Variable? Visit(ConstantDeclaration node)
    {
        if (!node.Expression.CtKnownValue.HasValue)
            throw new Exception($"Constant must have a compile time known value");

        var scope = node.Scope!;

        var newUserDefinedConstant = new UserDefinedConstant(node.Name, node.Expression.CtKnownValue.Value, node.IndexInScope);

        scope.AddUserConstant(newUserDefinedConstant);
        _flowContext.AllUserDefinedConstants.Add(newUserDefinedConstant);

        return null;
    }

    private Variable? Visit(VariableAssignment node)
    {
        if (!node.Scope!.TryGetUserVariable(node.Name, out var targetVariable))
            throw new Exception($"Variable {node.Name} is not defined");

        targetVariable.LastReassignedIndex = node.IndexInScope;
        node.Variable = targetVariable.Variable;

        var expressionVariable = VisitNode((Node)node.Expression);

        if (expressionVariable is not null)
            expressionVariable.LastReferencedIndex = node.IndexInScope;

        return null;
    }

    private Variable? Visit(UserDefinedValueAccess node)
    {
        if (node.Scope!.TryGetUserVariable(node.Name, out var userDefinedVariable))
        {
            node.Variable = userDefinedVariable.Variable;
            userDefinedVariable.LastReferencedIndex = node.IndexInScope;
            userDefinedVariable.Variable.LastReferencedIndex = node.IndexInScope;

            return userDefinedVariable.Variable;
        }

        if (node.Scope!.TryGetUserConstant(node.Name, out var userDefinedConstant))
        {
            node.CtKnownValue = userDefinedConstant.CtKnownValue;
            userDefinedConstant.LastReferencedIndex = node.IndexInScope;

            return null;
        }

        throw new Exception($"'{node.Name}' is not defined");
    }

    private Variable? Visit(PinDeclaration node)
    {
        if (node is PinDeclaration pin)
        {
            if (_root.DevicePinMap.Values.Contains(pin.Device))
                throw new Exception($"Pin {pin.Device} already defined");

            if (_root.DevicePinMap.ContainsKey(pin.Name))
                throw new Exception($"Pin {pin.Name} already defined");

            _root.DevicePinMap[pin.Name] = pin.Device;
        }

        return null;
    }

    private Variable? Visit(MethodDeclaration node)
    {
        foreach (Node item in node.Statements)
            VisitNode(item);

        return null;
    }

    private Variable? Visit(If node)
    {
        foreach (Node item in node.Expressions)
        {
            var innerVariable = VisitNode(item);

            if (innerVariable is not null)
                innerVariable.LastReferencedIndex = node.IndexInScope;
        }

        node.CurrentStatementsContainer = DataHolders.IfStatementsContainer.If;

        foreach (Node item in node.Statements)
            VisitNode(item);

        node.CurrentStatementsContainer = DataHolders.IfStatementsContainer.Else;

        foreach (Node item in node.Statements)
            VisitNode(item);

        return null;
    }
}
