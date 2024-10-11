using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.Instructions;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class Ic10CommandGenerator : ControlFlowTreeVisitorBase<object?>
{
    protected override Type VisitorType => typeof(Ic10CommandGenerator);

    public readonly List<Instruction> Instructions = new();
    private readonly FlowContext _flowContext;
    private readonly Stack<string> _continueLabels = new();
    private readonly Stack<string> _breakLabels = new();

    public Ic10CommandGenerator(FlowContext flowContext)
    {
        _flowContext = flowContext;
    }

    public List<Instruction> Visit(Root root)
    {
        VisitStatements(root.Statements);
        return Instructions;
    }

    private object? VisitStatements(List<IStatement> statements)
    {
        foreach (var item in statements)
            Visit((Node)item);

        return null;
    }

    private object? Visit(PinDeclaration node)
    {
        Instructions.Add(new DeviceAlias(node.Name, node.Device));

        return null;
    }

    private object? Visit(MethodDeclaration node)
    {
        Instructions.Add(new Label($"methodEnter{node.Name}"));

        if (node.Name == "Main")
        {
            // Ignore parameters
            VisitStatements(node.Statements);
            Instructions.Add(new Jump(JumpType.J, "9999")); // end program
            return null;
        }

        // Pop parameters
        foreach(string paramName in node.Parameters)
        {
            var variable = node.InnerScope!.UserDefinedVariables[paramName].Variable;
            Instructions.Add(new StackPop(variable));
        }

        // Push return address
        Instructions.Add(new StackPush("ra"));

        // r15 is used to store arrays sizes sum, so if there are arrays, we need to init it with 0
        if (node.ContainsArrays)
            Instructions.Add(new Move("r15", new Literal(0)));

        VisitStatements(node.Statements);

        Instructions.Add(new Label($"methodExit{node.Name}"));

        // If returns value, we do this in "return" statement instead
        if (node.ReturnType == MethodReturnType.Void && node.ContainsArrays)
        {
            var r15Expr = new DirectExpression("r15");
            var spExpr = new DirectExpression("sp");

            // Clear arrays from stack
            Instructions.Add(new Instructions.BinaryOperation(spExpr.Variable!, spExpr, r15Expr, BinaryOperationType.Sub));
        }

        Instructions.Add(new StackPop("ra"));

        // Push return value
        if (node.ReturnType == MethodReturnType.Real)
            Instructions.Add(new StackPush("r15"));

        Instructions.Add(new Jump(JumpType.J, "ra"));

        return null;
    }

    private object? Visit(MethodCall node)
    {
        // calculating parameters
        foreach (var item in node.Expressions.Reverse())
            Visit((Node)item);

        // save registers to stack             (Don't save params calculations)
        var usedRegisters = node.Scope!.GetUsedRegisters(node.IndexInScope + 1, node.IndexInScope + 1);

        if (node.Method!.ReturnType != MethodReturnType.Void)
            usedRegisters.Remove(node.Variable!.Register);

        if (node.Scope.Method!.ContainsArrays)
            usedRegisters.Add("r15");

        var declaredMethod = _flowContext.DeclaredMethods[node.Name];

        foreach (var register in ((IEnumerable<string>)usedRegisters).Reverse())
        Instructions.Add(new StackPush(register));

        // saving parameters to stack
        foreach (var item in node.Expressions.Reverse())
        {
            var value = item.CtKnownValue?.ToString() ?? item.Variable!.Register;
            Instructions.Add(new StackPush(value));
        }

        Instructions.Add(new Jump(JumpType.Jal, $"methodEnter{node.Name}"));

        if (declaredMethod.ReturnType != MethodReturnType.Void)
            Instructions.Add(new StackPop(node.Variable!.Register));

        // retrieve vars from stack
        foreach (var register in usedRegisters)
            Instructions.Add(new StackPop(register));

        return null;
    }

    private object? Visit(Return node)
    {
        var declaredMethod = node.Scope?.Method ?? throw new Exception($"Encountered return outside of a method");

        if (declaredMethod.ReturnType == MethodReturnType.Void)
        {
            Instructions.Add(new Jump(JumpType.J, $"methodExit{declaredMethod.Name}"));
        }
        else
        {
            // if return type is void, we do this in the exit part instead
            if (node.Scope.Method.ContainsArrays)
            {
                var r15Expr = new DirectExpression("r15");
                var spExpr = new DirectExpression("sp");

                // Clear arrays from stack
                Instructions.Add(new Instructions.BinaryOperation(spExpr.Variable!, spExpr, r15Expr, BinaryOperationType.Sub));
            }

            Visit((Node)node.Expression!);
            Instructions.Add(new Move("r15", node.Expression!));
            Instructions.Add(new Jump(JumpType.J, $"methodExit{declaredMethod.Name}"));
        }

        return null;
    }

    protected override object? Visit(While node)
    {
        var labelEnterInstruction = new Label($"While{node.Id}Enter");
        var labelExitInstruction = new Label($"While{node.Id}Exit");

        Instructions.Add(labelEnterInstruction);
        Visit((Node)node.Expression);
        Instructions.Add(new Jump(JumpType.Beqz, labelExitInstruction.Name, node.Expression.Render()));

        _continueLabels.Push(labelEnterInstruction.Name);
        _breakLabels.Push(labelExitInstruction.Name);
        VisitStatements(node.Statements);
        _breakLabels.Pop();
        _continueLabels.Pop();

        Instructions.Add(new Jump(JumpType.J, labelEnterInstruction.Name));
        Instructions.Add(labelExitInstruction);

        return null;
    }

    private object? Visit(Continue node)
    {
        if (!_continueLabels.Any())
            throw new Exception("Cuntinue must be inside a cycle");

        Instructions.Add(new Jump(JumpType.J, _continueLabels.Peek()));

        return null;
    }

    private object? Visit(Break node)
    {
        if (!_continueLabels.Any())
            throw new Exception("Break must be inside a cycle");

        Instructions.Add(new Jump(JumpType.J, _breakLabels.Peek()));

        return null;
    }

    protected override object? Visit(If node)
    {
        Visit((Node)node.Expression);

        var ifSkipLabelInstruction = new Label($"If{node.Id}Skip");

        Instructions.Add(new Jump(JumpType.Beqz, ifSkipLabelInstruction.Name, node.Expression.Render()));

        VisitStatements(node.IfStatements);


        Label? skipElseLabelInstruction = null;

        // Avoid entering ELSE block from IF block
        if (node.ElseStatements.Any())
        {
            skipElseLabelInstruction = new Label($"else{node.Id}Skip");
            Instructions.Add(new Jump(JumpType.J, skipElseLabelInstruction.Name));
        }

        Instructions.Add(ifSkipLabelInstruction);

        // ELSE
        if (node.ElseStatements.Any())
        {
            VisitStatements(node.ElseStatements);
            Instructions.Add(skipElseLabelInstruction!);
        }

        return null;
    }

    private object? Visit(Nodes.Yield node)
    {
        Instructions.Add(new Instructions.Yield());
        return null;
    }

    private object? Visit(VariableDeclaration node)
    {
        Visit((Node)node.Expression);
        Instructions.Add(new Move(node.Variable!, node.Expression));
        return null;
    }

    private object? Visit(UserDefinedValueAccess node)
    {
        return null;
    }

    private object? Visit(VariableAssignment node)
    {
        Visit((Node)node.Expression);
        Instructions.Add(new Move(node.Variable!, node.Expression));
        return null;
    }

    private object? Visit(Literal node)
    {
        return null;
    }

    private object? Visit(Nodes.MemberAssignment node)
    {
        if (node.SlotIndexExpr is not null)
            Visit((Node)node.SlotIndexExpr);

        Visit((Node)node.ValueExpression);

        if (node.SlotIndexExpr is null)
            Instructions.Add(new Instructions.MemberAssignment(node.Name, node.MemberName, node.ValueExpression));
        
        if (node.SlotIndexExpr is not null)
            Instructions.Add(new Instructions.MemberAssignment(node.Name, node.MemberName, node.SlotIndexExpr, node.ValueExpression));

        return null;
    }

    private object? Visit(Nodes.BinaryOperation node)
    {
        if (node.CtKnownValue.HasValue)
            return null;

        Visit((Node)node.Left);
        Visit((Node)node.Right);

        Instructions.Add(new Instructions.BinaryOperation(node.Variable!, node.Left, node.Right, node.Type));

        return null;
    }

    private object? Visit(Nodes.UnaryOperation node)
    {
        if (node.CtKnownValue.HasValue)
            return null;

        Visit((Node)node.Operand);

        Instructions.Add(new Instructions.UnaryOperation(node.Variable!, node.Operand, node.Type));

        return null;
    }

    private object? Visit(Nodes.MemberAccess node)
    {

        if (node.SlotIndexExpr is null)
            Instructions.Add(new Instructions.MemberAccess(node.Variable!, node.Name, node.MemberName));

        if (node.SlotIndexExpr is not null)
            Instructions.Add(new Instructions.MemberAccess(node.Variable!, node.Name, node.SlotIndexExpr, node.MemberName));

        return null;
    }

    private object? Visit(Nodes.DeviceWithIdAccess node)
    {
        Visit((Node)node.RefIdExpr);
        Instructions.Add(new Instructions.DeviceWithIdAccess(node.Variable!, node.RefIdExpr, node.Member));

        return null;
    }

    private object? Visit(Nodes.DeviceWithIndexAccess node)
    {
        Visit((Node)node.DeviceIndexExpr);

        if (node.SlotIndexExpr is not null)
            Visit((Node)node.SlotIndexExpr);

        if (node.SlotIndexExpr is null)
            Instructions.Add(new Instructions.DeviceWithIndexAccess(node.Variable!, node.DeviceIndexExpr, node.Member));

        if (node.SlotIndexExpr is not null)
            Instructions.Add(new Instructions.DeviceWithIndexAccess(node.Variable!, node.DeviceIndexExpr, node.SlotIndexExpr, node.Member));

        return null;
    }

    private object? Visit(Nodes.DeviceWithIdAssignment node)
    {
        Visit((Node)node.RefIdExpr);
        Visit((Node)node.ValueExpr);

        Instructions.Add(new Instructions.DeviceWithIdAssignment(node.RefIdExpr, node.Member, node.ValueExpr));

        return null;
    }

    private object? Visit(Nodes.DeviceWithIndexAssignment node)
    {
        Visit((Node)node.PinIndexExpr);

        if (node.SlotIndexExpr is not null)
            Visit((Node)node.SlotIndexExpr);

        Visit((Node)node.ValueExpr);

        if (node.SlotIndexExpr is null)
            Instructions.Add(new Instructions.DeviceWithIndexAssignment(node.PinIndexExpr, node.Member, node.ValueExpr));

        if (node.SlotIndexExpr is not null)
            Instructions.Add(new Instructions.DeviceWithIndexAssignment(node.PinIndexExpr, node.SlotIndexExpr, node.Member, node.ValueExpr));

        return null;
    }

    private object? Visit(ConstantDeclaration node)
    {
        return null;
    }

    private object? Visit(ArrayDeclaration node)
    {
        if (node.DeclarationType == ArrayDeclarationType.Size)
        {
            Visit((Node)node.SizeExpression);

            var spExpr = new DirectExpression("sp");
            Instructions.Add(new Move(node.AddressVariable!, spExpr));
            Instructions.Add(new Instructions.BinaryOperation(spExpr.Variable!, spExpr, node.SizeExpression, BinaryOperationType.Add));

            var r15Expr = new DirectExpression("r15");
            Instructions.Add(new Instructions.BinaryOperation(r15Expr.Variable!, r15Expr, node.SizeExpression, BinaryOperationType.Add));

            return null;
        }

        if (node.DeclarationType == ArrayDeclarationType.List)
        {
            var size = node.InitialElementExpressions.Count;

            var spExpr = new DirectExpression("sp");
            Instructions.Add(new Move(node.AddressVariable!, spExpr));

            foreach (var item in node.InitialElementExpressions)
            {
                Visit((Node)item);
                Instructions.Add(new StackPush(item));
            }

            var r15Expr = new DirectExpression("r15");
            Instructions.Add(new Instructions.BinaryOperation(r15Expr.Variable!, r15Expr, new Literal(size), BinaryOperationType.Add));

            return null;
        }

        throw new Exception($"Unexpected array declaration type {node.DeclarationType}");
    }

    private object? Visit(ArrayAssignment node)
    {
        Visit((Node)node.IndexExpression);
        Visit((Node)node.ValueExpression);

        var arrayAddr = new DirectExpression(node.ArrayAddressVariable!.Variable);
        Instructions.Add(new Instructions.BinaryOperation(node.Variable!, arrayAddr, node.IndexExpression, BinaryOperationType.Add));

        var elementAddr = new DirectExpression(node.Variable!);
        Instructions.Add(new StackPut("db", elementAddr, node.ValueExpression));

        return null;
    }

    private object? Visit(ArrayAccess node)
    {
        Visit((Node)node.IndexExpression);

        var arrayAddr = new DirectExpression(node.ArrayAddressVariable!.Variable);
        Instructions.Add(new Instructions.BinaryOperation(node.Variable!, arrayAddr, node.IndexExpression, BinaryOperationType.Add));

        var elementAddr = new DirectExpression(node.Variable!);
        Instructions.Add(new StackGet("db", node.Variable!, elementAddr));

        return null;
    }
}
