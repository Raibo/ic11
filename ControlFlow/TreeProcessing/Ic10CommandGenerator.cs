using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.Instructions;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;
using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.ControlFlow.TreeProcessing;
public class Ic10CommandGenerator : ControlFlowTreeVisitorBase<object?>
{
    protected override Type VisitorType => typeof(Ic10CommandGenerator);

    public readonly List<Instruction> Instructions = new();
    private readonly FlowContext _flowContext;
    private readonly Stack<string> _continueLabels = new();

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

        VisitStatements(node.Statements);

        Instructions.Add(new Label($"methodExit{node.Name}"));
        Instructions.Add(new StackPop("ra"));

        // Push return value
        if (node.ReturnType == MethodReturnType.Real)
            Instructions.Add(new StackPush("r15"));

        Instructions.Add(new Jump(JumpType.J, "ra"));

        return null;
    }

    private object? Visit(MethodCall node)
    {
        // save registers to stack
        var usedRegisters = node.Scope!.GetUsedRegisters(node.IndexInScope);
        var declaredMethod = _flowContext.DeclaredMethods[node.Name];

        foreach (var register in usedRegisters)
            Instructions.Add(new StackPush(register));

        // saving parameters to stack
        foreach (var item in node.Expressions.Reverse())
        {
            Visit((Node)item);
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

    protected override object? Visit(While node)
    {
        Visit((Node)node.Expression);

        var labelEnterInstruction = new Label($"While{node.Id}Enter");
        var labelExitInstruction = new Label($"While{node.Id}Exit");

        Instructions.Add(labelEnterInstruction);
        Visit((Node)node.Expression);
        Instructions.Add(new Jump(JumpType.Beqz, labelExitInstruction.Name, node.Expression.Render()));

        _continueLabels.Push(labelExitInstruction.Name);
        VisitStatements(node.Statements);
        _continueLabels.Pop();

        Instructions.Add(new Jump(JumpType.J, labelEnterInstruction.Name));
        Instructions.Add(labelExitInstruction);

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
        return null;
    }

    private object? Visit(Literal node)
    {
        return null;
    }

    private object? Visit(Nodes.MemberAssignment node)
    {
        Visit((Node)node.Expression);
        Instructions.Add(new Instructions.MemberAssignment(node.Name, node.MemberName, node.Expression));
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
        Instructions.Add(new Instructions.MemberAccess(node.Variable!, node.Name, node.MemberName));
        return null;
    }

    private object? Visit(Nodes.DeviceWithIdAccess node)
    {
        Visit((Node)node.RefIdExpr);
        Instructions.Add(new Instructions.DeviceWithIdAccess(node.Variable!, node.RefIdExpr, node.Member));

        return null;
    }

    private object? Visit(Nodes.DeviceWithIdAssignment node)
    {
        Visit((Node)node.RefIdExpr);
        Visit((Node)node.ValueExpr);

        Instructions.Add(new Instructions.DeviceWithIdAssignment(node.RefIdExpr, node.Member, node.ValueExpr));

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
            Visit((Node)node.Expression!);
            Instructions.Add(new Move("r15", node.Expression!));
            Instructions.Add(new Jump(JumpType.J, $"methodExit{declaredMethod.Name}"));
        }

        return null;
    }

    private object? Visit(ConstantDeclaration node)
    {
        return null;
    }
}
