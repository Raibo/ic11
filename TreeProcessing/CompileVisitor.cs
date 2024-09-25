using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ic11.TreeProcessing.Instructions;
using ic11.TreeProcessing.Results;
using ic11.TreeProcessing.Context;
using static Ic11Parser;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;

namespace ic11.TreeProcessing;
public class CompileVisitor : Ic11BaseVisitor<IValue>
{
    public CompileContext CompileContext;

    public CompileVisitor(CompileContext compileContext)
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

    public override IValue VisitDeviceWithIdAssignment([NotNull] DeviceWithIdAssignmentContext context)
    {
        var idValue = Visit(context.idExpr);
        var value = Visit(context.valueExpr);
        var deviceProperty = context.IDENTIFIER().GetText();

        var instruction = new DeviceIdWrite(CompileContext.CurrentScope, idValue, value, deviceProperty);
        CompileContext.Instructions.Add(instruction);

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

        var destination = CompileContext.ClaimTempVar();

        switch (context.op.Type)
        {
            case NEGATION:
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
            ADD => new BinaryAdd(CompileContext.CurrentScope, destination, operand1, operand2),
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

    public override IValue VisitDeviceIdAccess([NotNull] DeviceIdAccessContext context)
    {
        var deviceProperty = context.IDENTIFIER().GetText();
        var deviceIdValue = Visit(context.expression());

        var destination = CompileContext.ClaimTempVar();

        CompileContext.Instructions.Add(new DeviceIdRead(CompileContext.CurrentScope, destination, deviceIdValue, deviceProperty));

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
    public override IValue VisitFunctionCall([NotNull] Ic11Parser.FunctionCallContext context) =>
        VisitAnyFunctionCall(context, null);

    public override IValue VisitFunctionCallStatement([NotNull] FunctionCallStatementContext context) =>
        VisitAnyFunctionCall(null, context);

    public IValue VisitAnyFunctionCall(Ic11Parser.FunctionCallContext? context1, FunctionCallStatementContext? context2)
    {
        var methodName = context1?.IDENTIFIER().GetText() ?? context2!.IDENTIFIER().GetText();
        var expressions = context1?.expression() ?? context2!.expression();

        if (!CompileContext.DeclaredMethods.TryGetValue(methodName, out var declaredMethod))
            throw new Exception($"Method {methodName} is not defined");

        if (declaredMethod.ParamCount != expressions.Length)
            throw new Exception($"Method {methodName} has {declaredMethod.ParamCount} parameters, but is called with {expressions.Length}");

        // save vars to stack
        foreach (var variable in ((IEnumerable<Variable>)CompileContext.Variables).Reverse())
        {
            var saveVarInstruction = new StackPush(CompileContext.CurrentScope, variable)
            {
                Description = new InstructionDescription() { Purpose = InstructionPurpose.SaveVariableBeforeMethodCall, Variable1 = variable }
            };

            CompileContext.Instructions.Add(saveVarInstruction);
        }

        // saving parameters to stack
        CompileContext.EnterScope(); // to avoid having temp variables in scope
        for(int i = declaredMethod.ParamCount - 1; i >= 0; i--)
        {
            var paramValue = Visit(expressions[i]);

            var saveParamInstruction = new StackPush(CompileContext.CurrentScope, paramValue)
            {
                Description = new InstructionDescription() { Purpose = InstructionPurpose.SaveParameterBeforeMethodCall, Value1 = paramValue }
            };

            paramValue.UpdateUsage(CompileContext.Instructions.Count);
            CompileContext.Instructions.Add(saveParamInstruction);
        }
        CompileContext.ExitScope();

        CompileContext.Instructions.Add(new JumpAl(CompileContext.CurrentScope, $"methodEnter{methodName}"));

        Variable? returnValue = null;

        if (declaredMethod.ReturnsValue)
        {
            returnValue = CompileContext.ClaimTempVar();
            CompileContext.Instructions.Add(new StackPop(CompileContext.CurrentScope, returnValue));
        }

        // retrieve vars from stack
        foreach (var variable in CompileContext.Variables)
        {
            var loadVarInstruction = new StackPop(CompileContext.CurrentScope, variable)
            {
                Description = new InstructionDescription() { Purpose = InstructionPurpose.RestoreVariableAfterMethodCall, Variable1 = variable }
            };

            CompileContext.Instructions.Add(loadVarInstruction);
        }

        return returnValue;
    }

    public override IValue VisitFunction([NotNull] Ic11Parser.FunctionContext context)
    {
        var identifiers = context.IDENTIFIER();
        var parameters = new Span<ITerminalNode>(identifiers, 1, identifiers.Length - 1);
        var name = identifiers[0].GetText();
        var block = context.block();
        var declaredMethod = CompileContext.DeclaredMethods[name];

        CompileContext.EnterScope(declaredMethod);

        CompileContext.Instructions.Add(new Label(CompileContext.CurrentScope, $"methodEnter{name}"));

        if (name == "Main")
        {
            // Ignore parameters
            VisitChildren(context);
            CompileContext.Instructions.Add(new Jump(CompileContext.CurrentScope, "9999")); // end program
            return null;
        }

        // Pop parameters
        for (int i = 0; i < declaredMethod.ParamCount; i++)
        {
            var paramName = parameters[i].GetText();
            var variable = CompileContext.ClaimTempVar();
            CompileContext.UserValuesMap[paramName] = variable;
            CompileContext.Instructions.Add(new StackPop(CompileContext.CurrentScope, variable));
        }

        // Push return address
        CompileContext.Instructions.Add(new StackPush(CompileContext.CurrentScope, new DirectRegister("ra")));

        Visit(block);

        CompileContext.Instructions.Add(new Label(CompileContext.CurrentScope, $"methodExit{name}"));

        CompileContext.Instructions.Add(new StackPop(CompileContext.CurrentScope, new DirectRegister("ra")));

        if (declaredMethod.ReturnsValue)
        {
            CompileContext.Instructions.Add(new StackPush(CompileContext.CurrentScope, new DirectRegister("r15")));
        }

        CompileContext.Instructions.Add(new Jump(CompileContext.CurrentScope, "ra"));

        CompileContext.ExitScope();

        return null;
    }


    public override IValue VisitReturnStatement([NotNull] ReturnStatementContext context)
    {
        var declaredMethod = CompileContext.CurrentScope.Method;

        if (declaredMethod is null)
            throw new Exception($"Encountered return outside of a method");

        if (declaredMethod.ReturnsValue)
            throw new Exception($"Return must specify a value");

        CompileContext.Instructions.Add(new Jump(CompileContext.CurrentScope, $"methodExit{declaredMethod.Name}"));

        return null;
    }

    public override IValue VisitReturnValueStatement([NotNull] ReturnValueStatementContext context)
    {
        var declaredMethod = CompileContext.CurrentScope.Method;

        if (declaredMethod is null)
            throw new Exception($"Encountered return outside of a method");

        if (!declaredMethod.ReturnsValue)
            throw new Exception($"Return must not specify a value");

        var expression = context.expression();

        var value = Visit(expression);
        CompileContext.Instructions.Add(new Move(CompileContext.CurrentScope, new DirectRegister("r15"), value));
        CompileContext.Instructions.Add(new Jump(CompileContext.CurrentScope, $"methodExit{declaredMethod.Name}"));

        return null;
    }

    public override IValue VisitChildren(IRuleNode node) => base.VisitChildren(node);
    public override IValue VisitDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context) => base.VisitDelimitedStatement(context);
    public override IValue VisitErrorNode(IErrorNode node) => base.VisitErrorNode(node);
    public override IValue VisitParenthesis([NotNull] Ic11Parser.ParenthesisContext context) => base.VisitParenthesis(context);
    public override IValue VisitStatement([NotNull] Ic11Parser.StatementContext context) => base.VisitStatement(context);
    public override IValue VisitTerminal(ITerminalNode node) => base.VisitTerminal(node);
    public override IValue VisitUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context) => base.VisitUndelimitedStatement(context);
    protected override IValue AggregateResult(IValue aggregate, IValue nextResult) => base.AggregateResult(aggregate, nextResult);
}
