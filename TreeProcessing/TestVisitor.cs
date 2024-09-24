using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ic11.TreeProcessing.Instructions;
using ic11.TreeProcessing.Results;
using static Ic11Parser;

namespace ic11.TreeProcessing;
public class TestVisitor : Ic11BaseVisitor<IValue>
{
    public ProgramContext ProgramContext;

    public TestVisitor(ProgramContext programContext)
    {
        ProgramContext = programContext;
    }

    public override IValue Visit(IParseTree tree) => base.Visit(tree);

    public override IValue VisitDeclaration([NotNull] Ic11Parser.DeclarationContext context)
    {
        ProgramContext.Instructions.Add(new PinName(context.IDENTIFIER().GetText(), context.PINID().GetText()));
        return null;
    }

    public override IValue VisitAssignment([NotNull] Ic11Parser.AssignmentContext context)
    {
        var value = Visit(context.expression());
        // Device write
        var instruction = new DeviceWrite(context.IDENTIFIER()[0].GetText(), context.IDENTIFIER()[1].GetText(), value);
        ProgramContext.Instructions.Add(instruction);

        return null;
    }

    public override IValue VisitVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context)
    {
        var value = Visit(context.expression());
        ProgramContext.UserValues.Add(new UserValue(context.IDENTIFIER().GetText(), value));
        return null;
    }

    public override IValue VisitFunction([NotNull] Ic11Parser.FunctionContext context)
    {
        VisitChildren(context);

        return null;
    }

    public override IValue VisitYieldStatement([NotNull] Ic11Parser.YieldStatementContext context)
    {
        ProgramContext.Instructions.Add(new Yield());
        return null;
    }

    public override IValue VisitWhileStatement([NotNull] Ic11Parser.WhileStatementContext context)
    {
        var whileCount = ++ProgramContext.WhileCount;

        var labelEnterInstruction = new Label($"While{whileCount}Enter");
        var labelExitInstruction = new Label($"While{whileCount}Exit");

        ProgramContext.CycleContinueLabels.Push(labelEnterInstruction.Name);

        ProgramContext.Instructions.Add(labelEnterInstruction);
        var conditionValue = Visit(context.expression());

        var skipInstruction = new JumpLez(labelExitInstruction.Name, conditionValue);
        ProgramContext.Instructions.Add(skipInstruction);

        Visit(context.block());

        var cycleJumpInstruction = new Jump(labelEnterInstruction.Name);
        ProgramContext.Instructions.Add(cycleJumpInstruction);
        ProgramContext.Instructions.Add(labelExitInstruction);

        ProgramContext.CycleContinueLabels.Pop();

        return null;
    }

    public override IValue VisitIfStatement([NotNull] Ic11Parser.IfStatementContext context)
    {
        var ifCount = ++ProgramContext.IfCount;

        var blocks = context.block();

        var conditionValue = Visit(context.expression());
        var ifSkipLabelInstruction = new Label($"If{ifCount}Skip");

        var ifSkipInstruction = new JumpLez(ifSkipLabelInstruction.Name, conditionValue);
        ProgramContext.Instructions.Add(ifSkipInstruction);

        Visit(blocks[0]);

        Label skipElseLabelInstruction = null;

        // Avoid entering ELSE block from IF block
        if (blocks.Length == 2)
        {
            skipElseLabelInstruction = new Label($"else{ifCount}Skip");
            var elseSkipInstruction = new Jump(skipElseLabelInstruction.Name);
            ProgramContext.Instructions.Add(elseSkipInstruction);
        }

        ProgramContext.Instructions.Add(ifSkipLabelInstruction);

        // ELSE
        if (blocks.Length == 2)
        {
            Visit(blocks[1]);
            ProgramContext.Instructions.Add(skipElseLabelInstruction);
        }

        return null;
    }

    public override IValue VisitUnaryOp([NotNull] Ic11Parser.UnaryOpContext context)
    {
        switch (context.op.Type)
        {
            case NEGATION:
                var destination = ProgramContext.ClaimTempVar();
                var value = Visit(context.operand);
                ProgramContext.Instructions.Add(new UnaryNot(destination, value));
                return destination;
            default:
                throw new NotImplementedException("Only unary NOT is implemented");
        }
    }

    public override IValue VisitBinaryOp([NotNull] Ic11Parser.BinaryOpContext context)
    {
        var operand1 = Visit(context.left);
        var operand2 = Visit(context.right);
        var destination = ProgramContext.ClaimTempVar();

        IInstruction instruction = context.op.Type switch
        {
            SUB => new BinarySub(destination, operand1, operand2),
            AND => new BinaryAnd(destination, operand1, operand2),
            LE => new BinaryLe(destination, operand1, operand2),
            _ => throw new NotImplementedException(),
        };

        ProgramContext.Instructions.Add(instruction);

        return destination;
    }

    public override IValue VisitMemberAccess([NotNull] Ic11Parser.MemberAccessContext context)
    {
        var device = context.IDENTIFIER()[0].GetText();
        var deviceProperty = context.IDENTIFIER()[1].GetText();

        var destination = ProgramContext.ClaimTempVar();

        ProgramContext.Instructions.Add(new DeviceRead(destination, device, deviceProperty));

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

        var userDefined = ProgramContext.UserValues.First(x => x.Name == name);
        return userDefined.Value;
    }

    public override IValue VisitContinueStatement([NotNull] ContinueStatementContext context)
    {
        var continueLabel = ProgramContext.CycleContinueLabels.Peek();
        ProgramContext.Instructions.Add(new Jump(continueLabel));
        return null;
    }

    public override IValue VisitBlock([NotNull] Ic11Parser.BlockContext context) => base.VisitBlock(context);
    public override IValue VisitChildren(IRuleNode node) => base.VisitChildren(node);
    public override IValue VisitDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context) => base.VisitDelimitedStatement(context);
    public override IValue VisitErrorNode(IErrorNode node) => base.VisitErrorNode(node);
    public override IValue VisitFunctionCall([NotNull] Ic11Parser.FunctionCallContext context) => base.VisitFunctionCall(context);
    public override IValue VisitParenthesis([NotNull] Ic11Parser.ParenthesisContext context) => base.VisitParenthesis(context);
    public override IValue VisitProgram([NotNull] Ic11Parser.ProgramContext context) => base.VisitProgram(context);
    public override IValue VisitStatement([NotNull] Ic11Parser.StatementContext context) => base.VisitStatement(context);
    public override IValue VisitTerminal(ITerminalNode node) => base.VisitTerminal(node);
    public override IValue VisitUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context) => base.VisitUndelimitedStatement(context);
    protected override IValue AggregateResult(IValue aggregate, IValue nextResult) => base.AggregateResult(aggregate, nextResult);
    public override IValue VisitReturnStatement([NotNull] ReturnStatementContext context) => base.VisitReturnStatement(context);
}
