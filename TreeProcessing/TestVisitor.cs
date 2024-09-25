using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ic11.TreeProcessing.Instructions;
using ic11.TreeProcessing.Results;
using ic11.TreeProcessing.Context;
using static Ic11Parser;

namespace ic11.TreeProcessing;
public class TestVisitor : Ic11BaseVisitor<IValue>
{
    public CompileContext CompileContext;

    public TestVisitor(CompileContext compileContext)
    {
        CompileContext = compileContext;
    }

    public override IValue Visit(IParseTree tree) => base.Visit(tree);

    public override IValue VisitDeclaration([NotNull] Ic11Parser.DeclarationContext context)
    {
        CompileContext.Instructions.Add(new PinName(CompileContext.CurrentScope, context.IDENTIFIER().GetText(), context.PINID().GetText()));
        return null;
    }

    public override IValue VisitAssignment([NotNull] Ic11Parser.AssignmentContext context)
    {
        var value = Visit(context.expression());
        var identifiers = context.IDENTIFIER();

        // Assignment
        if (identifiers.Length == 1)
        {
            var variableName = identifiers[0].GetText();

            var haveVariable = CompileContext.UserValuesMap.TryGetValue(variableName, out var userValue);

            if (!haveVariable)
                throw new Exception($"Variable {variableName} is not defined");

            if (userValue is Variable userVariable)
                userVariable.UpdateUsage(CompileContext.Instructions.Count - 1);

            CompileContext.UserValuesMap[variableName] = value;
        }

        // Device write
        if (identifiers.Length == 2)
        {
            var instruction = new DeviceWrite(CompileContext.CurrentScope, context.IDENTIFIER()[0].GetText(), context.IDENTIFIER()[1].GetText(), value);
            CompileContext.Instructions.Add(instruction);
        }

        if (identifiers.Length > 2)
        {
            throw new NotImplementedException();
        }

        return null;
    }

    public override IValue VisitVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context)
    {
        var value = Visit(context.expression());
        CompileContext.UserValuesMap[context.IDENTIFIER().GetText()] = value;
        return null;
    }

    public override IValue VisitYieldStatement([NotNull] Ic11Parser.YieldStatementContext context)
    {
        CompileContext.Instructions.Add(new Yield(CompileContext.CurrentScope));
        return null;
    }

    public override IValue VisitWhileStatement([NotNull] Ic11Parser.WhileStatementContext context)
    {
        var whileCount = ++CompileContext.WhileCount;

        var labelEnterInstruction = new Label(CompileContext.CurrentScope, $"While{whileCount}Enter");
        var labelExitInstruction = new Label(CompileContext.CurrentScope, $"While{whileCount}Exit");

        CompileContext.CycleContinueLabels.Push(labelEnterInstruction.Name);

        CompileContext.EnterScope();
        CompileContext.Instructions.Add(labelEnterInstruction);
        var conditionValue = Visit(context.expression());
        conditionValue.UpdateUsage(CompileContext.Instructions.Count);

        var skipInstruction = new JumpLez(CompileContext.CurrentScope, labelExitInstruction.Name, conditionValue);
        CompileContext.Instructions.Add(skipInstruction);

        Visit(context.block());
        CompileContext.ExitScope();

        var cycleJumpInstruction = new Jump(CompileContext.CurrentScope, labelEnterInstruction.Name);
        CompileContext.Instructions.Add(cycleJumpInstruction);
        CompileContext.Instructions.Add(labelExitInstruction);

        CompileContext.CycleContinueLabels.Pop();

        return null;
    }

    public override IValue VisitIfStatement([NotNull] Ic11Parser.IfStatementContext context)
    {
        var ifCount = ++CompileContext.IfCount;

        var blocks = context.block();

        CompileContext.EnterScope();

        var conditionValue = Visit(context.expression());
        conditionValue.UpdateUsage(CompileContext.Instructions.Count);

        var ifSkipLabelInstruction = new Label(CompileContext.CurrentScope, $"If{ifCount}Skip");

        var ifSkipInstruction = new JumpLez(CompileContext.CurrentScope, ifSkipLabelInstruction.Name, conditionValue);
        CompileContext.Instructions.Add(ifSkipInstruction);

        Visit(blocks[0]);
        CompileContext.ExitScope();

        Label skipElseLabelInstruction = null;

        // Avoid entering ELSE block from IF block
        if (blocks.Length == 2)
        {
            skipElseLabelInstruction = new Label(CompileContext.CurrentScope, $"else{ifCount}Skip");
            var elseSkipInstruction = new Jump(CompileContext.CurrentScope, skipElseLabelInstruction.Name);
            CompileContext.Instructions.Add(elseSkipInstruction);
        }

        CompileContext.Instructions.Add(ifSkipLabelInstruction);

        // ELSE
        if (blocks.Length == 2)
        {
            CompileContext.EnterScope();
            Visit(blocks[1]);
            CompileContext.ExitScope();
            CompileContext.Instructions.Add(skipElseLabelInstruction);
        }

        return null;
    }

    public override IValue VisitUnaryOp([NotNull] Ic11Parser.UnaryOpContext context)
    {
        var value = Visit(context.operand);
        value.UpdateUsage(CompileContext.Instructions.Count);

        switch (context.op.Type)
        {
            case NEGATION:
                var destination = CompileContext.ClaimTempVar();
                CompileContext.Instructions.Add(new UnaryNot(CompileContext.CurrentScope, destination, value));
                return destination;
            default:
                throw new NotImplementedException("Only unary NOT is implemented");
        }
    }

    public override IValue VisitBinaryOp([NotNull] Ic11Parser.BinaryOpContext context)
    {
        var operand1 = Visit(context.left);
        var operand2 = Visit(context.right);

        operand1.UpdateUsage(CompileContext.Instructions.Count);
        operand2.UpdateUsage(CompileContext.Instructions.Count);

        var destination = CompileContext.ClaimTempVar();

        InstructionBase instruction = context.op.Type switch
        {
            SUB => new BinarySub(CompileContext.CurrentScope, destination, operand1, operand2),
            AND => new BinaryAnd(CompileContext.CurrentScope, destination, operand1, operand2),
            LE => new BinaryLe(CompileContext.CurrentScope, destination, operand1, operand2),
            _ => throw new NotImplementedException(),
        };

        CompileContext.Instructions.Add(instruction);

        return destination;
    }

    public override IValue VisitMemberAccess([NotNull] Ic11Parser.MemberAccessContext context)
    {
        var device = context.IDENTIFIER()[0].GetText();
        var deviceProperty = context.IDENTIFIER()[1].GetText();

        var destination = CompileContext.ClaimTempVar();

        CompileContext.Instructions.Add(new DeviceRead(CompileContext.CurrentScope, destination, device, deviceProperty));

        return destination;
    }

    public override IValue VisitLiteral([NotNull] Ic11Parser.LiteralContext context)
    {
        var value = context.GetText();

        if (value == "true")
            value = "1";

        if (value == "false")
            value = "0";

        var parsedValue = double.Parse(value);

        return new Constant(parsedValue);
    }

    public override IValue VisitIdentifier([NotNull] Ic11Parser.IdentifierContext context)
    {
        var name = context.IDENTIFIER().GetText();
        var value = CompileContext.UserValuesMap[name];

        value.UpdateUsage(CompileContext.Instructions.Count);

        return value;
    }

    public override IValue VisitContinueStatement([NotNull] ContinueStatementContext context)
    {
        var continueLabel = CompileContext.CycleContinueLabels.Peek();
        CompileContext.Instructions.Add(new Jump(CompileContext.CurrentScope, continueLabel));
        return null;
    }

    public override IValue VisitBlock([NotNull] Ic11Parser.BlockContext context)
    {
        var value = base.VisitBlock(context);
        return value;
    }

    public override IValue VisitProgram([NotNull] Ic11Parser.ProgramContext context)
    {
        CompileContext.EnterScope();
        var value = base.VisitProgram(context);
        CompileContext.ExitScope();

        return value;
    }

    public override IValue VisitFunction([NotNull] Ic11Parser.FunctionContext context)
    {
        CompileContext.EnterScope();
        var value = VisitChildren(context);
        CompileContext.ExitScope();

        return value;
    }

    public override IValue VisitChildren(IRuleNode node) => base.VisitChildren(node);
    public override IValue VisitDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context) => base.VisitDelimitedStatement(context);
    public override IValue VisitErrorNode(IErrorNode node) => base.VisitErrorNode(node);
    public override IValue VisitFunctionCall([NotNull] Ic11Parser.FunctionCallContext context) => base.VisitFunctionCall(context);
    public override IValue VisitParenthesis([NotNull] Ic11Parser.ParenthesisContext context) => base.VisitParenthesis(context);
    public override IValue VisitStatement([NotNull] Ic11Parser.StatementContext context) => base.VisitStatement(context);
    public override IValue VisitTerminal(ITerminalNode node) => base.VisitTerminal(node);
    public override IValue VisitUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context) => base.VisitUndelimitedStatement(context);
    protected override IValue AggregateResult(IValue aggregate, IValue nextResult) => base.AggregateResult(aggregate, nextResult);
    public override IValue VisitReturnStatement([NotNull] ReturnStatementContext context) => base.VisitReturnStatement(context);
}
