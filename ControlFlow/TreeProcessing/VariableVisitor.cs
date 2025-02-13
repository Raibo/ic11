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
        typeof(For),
        typeof(MethodDeclaration),
        typeof(ArrayDeclaration),
        typeof(ArrayAssignment),
        typeof(ArrayAccess),
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
            // If node needs to hold multiple results at once, it has increased index size
            var addedIndex = node.IndexSize <= 1 ? 0 : 1;

            foreach (Node item in ec.Expressions)
            {
                var innerVariable = VisitNode(item);

                if (innerVariable is not null)
                    innerVariable.LastReferencedIndex = node.IndexInScope + addedIndex;
            }
        }

        if (node is IExpression ex)
        {
            if (ex.CtKnownValue is null)
                ex.Variable = node.Scope!.ClaimNewVariable(node.IndexInScope);

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

        node.Variable = node.Scope!.ClaimNewVariable(node.IndexInScope);

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
        targetVariable.LastReferencedIndex = node.IndexInScope;

        targetVariable.Variable.LastReassignedIndex = node.IndexInScope;
        targetVariable.Variable.LastReferencedIndex = node.IndexInScope;

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
                innerVariable.LastReferencedIndex = Math.Max(node.IndexInScope, innerVariable.DeclareIndex);
        }

        node.CurrentStatementsContainer = DataHolders.IfStatementsContainer.If;

        foreach (Node item in node.Statements)
            VisitNode(item);

        node.CurrentStatementsContainer = DataHolders.IfStatementsContainer.Else;

        foreach (Node item in node.Statements)
            VisitNode(item);

        return null;
    }

    private Variable? Visit(For node)
    {
        IEnumerable<IStatement> innerStatements = node.Statements;

        if (node.HasStatement1)
        {
            VisitNode((Node)node.Statements.First());
            innerStatements = innerStatements.Skip(1);
        }

        var innerVariable = VisitNode((Node)node.Expression);

        if (innerVariable is not null)
            innerVariable.LastReferencedIndex = Math.Max(node.IndexInScope, innerVariable.DeclareIndex);

        foreach (Node item in innerStatements)
            VisitNode(item);

        return null;
    }

    private Variable? Visit(ArrayDeclaration node)
    {
        node.AddressVariable = node.Scope!.ClaimNewVariable(node.IndexInScope);

        var newUserArray = new UserDefinedVariable(node.Name, node.AddressVariable, node.IndexInScope, false);
        node.Scope!.AddUserVariable(newUserArray);
        _flowContext.AllUserDefinedVariables.Add(newUserArray);

        if (node.DeclarationType == DataHolders.ArrayDeclarationType.Size)
        {
            var sizeVariable = VisitNode((Node)node.SizeExpression);

            if (sizeVariable is not null)
                sizeVariable.LastReferencedIndex = sizeVariable.DeclareIndex; // Gets used immediately after calculated
        }

        if (node.DeclarationType == DataHolders.ArrayDeclarationType.List)
        {
            foreach (Node item in node.InitialElementExpressions!)
            {
                var innerVariable = VisitNode(item);

                if (innerVariable is not null)
                    innerVariable.LastReferencedIndex = innerVariable.DeclareIndex; // Gets used immediately after calculated
            }
        }

        return null;
    }

    private Variable? Visit(ArrayAssignment node)
    {
        if (!node.Scope!.TryGetUserVariable(node.Name, out var addressVariable))
            throw new Exception($"{node.Name} is not defined");

        node.ArrayAddressVariable = addressVariable;
        addressVariable.LastReferencedIndex = node.IndexInScope;
        addressVariable.Variable.LastReferencedIndex = node.IndexInScope;

        node.Variable = node.Scope!.ClaimNewVariable(node.IndexInScope);
        node.Variable.LastReferencedIndex = node.IndexInScope;

        var valueExprVariable = VisitNode((Node)node.ValueExpression);

        if (valueExprVariable is not null)
            valueExprVariable.LastReferencedIndex = node.IndexInScope;

        var indexExprVariable = VisitNode((Node)node.IndexExpression);

        if (indexExprVariable is not null)
            indexExprVariable.LastReferencedIndex = node.IndexInScope + 1;

        return null;
    }

    private Variable? Visit(ArrayAccess node)
    {
        if (!node.Scope!.TryGetUserVariable(node.Name, out var addressVariable))
            throw new Exception($"Array '{node.Name}' is not defined");

        node.ArrayAddressVariable = addressVariable;
        addressVariable.LastReferencedIndex = node.IndexInScope;
        addressVariable.Variable.LastReferencedIndex = node.IndexInScope;

        var indexVariable = VisitNode((Node)node.IndexExpression);

        if (indexVariable is not null)
            indexVariable.LastReferencedIndex = node.IndexInScope;

        node.Variable = node.Scope!.ClaimNewVariable(node.IndexInScope);
        node.Variable.LastReferencedIndex = node.IndexInScope;

        return node.Variable;
    }
}
