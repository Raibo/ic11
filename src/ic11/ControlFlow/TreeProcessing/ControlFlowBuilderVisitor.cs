using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;
using System.Globalization;
using static Ic11Parser;

namespace ic11.ControlFlow.TreeProcessing;
public class ControlFlowBuilderVisitor : Ic11BaseVisitor<INodeExpression?>
{
    public FlowContext FlowContext;

    private INode CurrentNode
    {
        get { return FlowContext.CurrentNode; }
        set { FlowContext.CurrentNode = value; }
    }

    private void AddToStatements(IStatement node)
    {
        if (CurrentNode is not IStatementsContainer statementsContainer)
            throw new Exception($"Unexpected statement {node?.GetType().Name}");

        statementsContainer.AddToStatements(node);
    }

    public ControlFlowBuilderVisitor(FlowContext flowContext)
    {
        FlowContext = flowContext;
    }

    public override INodeExpression? Visit(IParseTree tree) => base.Visit(tree);

    public INodeExpression Visit(ExpressionContext context) => base.Visit(context)!;

    public override INodeExpression? VisitDeclaration([NotNull] DeclarationContext context)
    {
        if (CurrentNode is not Root root)
            throw new Exception($"Pin declaration must be top level statement");

        var newNode = new PinDeclaration(context.IDENTIFIER().GetText(), context.PINID().GetText());
        root.Statements.Add(newNode);

        return null;
    }

    public override INodeExpression? VisitFunction([NotNull] FunctionContext context)
    {
        var identifiers = context.IDENTIFIER();
        var name = identifiers[0].GetText();

        var parameters = identifiers.Skip(1)
            .Select(i => i.GetText())
            .ToList();

        var block = context.block();

        var returnType = context.retType.Text switch
        {
            "void" => MethodReturnType.Void,
            "real" => MethodReturnType.Real,
            _ => throw new Exception($"Unrecognized method return type {context.retType.Text}. Supported: void, real."),
        };

        var newNode = new MethodDeclaration(name, returnType, parameters);

        if (CurrentNode is not Root root)
            throw new Exception($"Method declaration must be top level statement");

        root.Statements.Add(newNode);

        CurrentNode = newNode;
        Visit(block);
        CurrentNode = root;

        return null;
    }

    public override INodeExpression? VisitYieldStatement([NotNull] YieldStatementContext context)
    {
        var newNode = new StatementParam0("yield");
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitHcfStatement([NotNull] HcfStatementContext context)
    {
        var newNode = new StatementParam0("hcf");
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitDeviceStackClear([NotNull] DeviceStackClearContext context)
    {
        var device = context.identifier.Text;

        if (context.identifier.Type == BASE_DEVICE)
            device = "db";

        var newNode = new StatementParam0($"clr {device}");
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitDeviceWithIdStackClear([NotNull] DeviceWithIdStackClearContext context)
    {
        var expression = Visit(context.deviceIdxExpr)!;
        var newNode = new StatementParam1("clrd", expression);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitSleepStatement([NotNull] SleepStatementContext context)
    {
        var expression = Visit(context.expression())!;
        var newNode = new StatementParam1("sleep", expression);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitVariableDeclaration([NotNull] VariableDeclarationContext context)
    {
        var variableName = context.IDENTIFIER().GetText();
        var expression = Visit(context.expression())!;
        var newNode = new VariableDeclaration(variableName, expression);

        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitConstantDeclaration([NotNull] ConstantDeclarationContext context)
    {
        var constantName = context.IDENTIFIER().GetText();
        var expression = Visit(context.expression())!;
        var newNode = new ConstantDeclaration(constantName, expression);

        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression VisitLiteral([NotNull] LiteralContext context)
    {
        var value = context.GetText();

        var number = context.type.Type switch
        {
            STRING_LITERAL => OperationHelper.ToASCII(value.AsSpan(1..^1)),
            HASH_LITERAL => OperationHelper.Hash(value.Trim('"')),
            INTEGER_HEX => OperationHelper.ParseHex(value),
            INTEGER_BINARY => OperationHelper.ParseBinary(value),
            _ when value == "true" => 1m,
            _ when value == "false" => 0m,
            _ => decimal.Parse(value, CultureInfo.InvariantCulture),
        };

        return new Literal(number);
    }

    public override INodeExpression? VisitIfStatement([NotNull] IfStatementContext context)
    {
        var hasElsePart = context.ELSE() is not null;

        var blocks = context.block();
        var rawStatements = context.statement();

        List<IParseTree> codeContents = blocks.Any()
            ? blocks.Select(b => (IParseTree)b).ToList()
            : rawStatements.Select(s => (IParseTree)s).ToList();

        if (hasElsePart && blocks.Length == 1 && rawStatements.Length == 1)
            throw new Exception($"Inconsistent if-else satement. Either use blocks or raw statements for both parts.");

        var expression = Visit(context.expression())!;

        var newNode = new If(expression);
        AddToStatements(newNode);

        CurrentNode = newNode;
        newNode.CurrentStatementsContainer = IfStatementsContainer.If;
        Visit(codeContents[0]);

        if (hasElsePart)
        {
            newNode.CurrentStatementsContainer = IfStatementsContainer.Else;
            Visit(codeContents[1]);
        }

        CurrentNode = newNode.Parent!;

        return null;
    }

    public override INodeExpression? VisitMemberAssignment([NotNull] MemberAssignmentContext context)
    {
        var valueExpr = Visit(context.valueExpr)!;

        var member = context.member.Text;
        var device = context.identifier.Text;

        if (context.identifier.Type == BASE_DEVICE)
            device = "db";

        var newNode = new MemberAssignment(device, member, valueExpr);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitMemberExtendedAssignment([NotNull] MemberExtendedAssignmentContext context)
    {
        var valueExpr = Visit(context.valueExpr)!;
        var targetIdxExpr = Visit(context.targetIdxExpr)!;

        var member = context.member?.Text;
        var device = context.identifier.Text;

        if (context.identifier.Type == BASE_DEVICE)
            device = "db";

        var newNode = new MemberAssignment(device, GetDeviceTarget(context.prop.Type), member, targetIdxExpr, valueExpr);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression VisitMemberAccess([NotNull] MemberAccessContext context)
    {
        var member = context.member.Text;
        var device = context.identifier.Text;

        if (context.identifier.Type == BASE_DEVICE)
            device = "db";

        var newNode = new MemberAccess(device, member);

        return newNode;
    }

    public override INodeExpression? VisitExtendedMemberAccess([NotNull] ExtendedMemberAccessContext context)
    {
        var targetIdxExpr = Visit(context.targetIdxExpr)!;

        var member = context.member?.Text;
        var device = context.identifier.Text;

        if (context.identifier.Type == BASE_DEVICE)
            device = "db";

        var target = GetDeviceTarget(context.prop.Type);

        var newNode = new MemberAccess(device, target, targetIdxExpr, member);

        return newNode;
    }

    public override INodeExpression? VisitBatchAccess([NotNull] BatchAccessContext context)
    {
        var typeHash = Visit(context.deviceTypeHashExpr)!;

        var nameHash = context.deviceNameHashExpr is null
            ? null
            : Visit(context.deviceNameHashExpr)!;

        var targetIdx = context.targetIdxExpr is null
            ? null
            : Visit(context.targetIdxExpr)!;

        var deviceProperty = context.member.Text;
        var batchMode = context.batchMode.Text;

        var target = context.prop is null
            ? DeviceTarget.Device
            : GetDeviceTarget(context.prop.Type);

        var newNode = new BatchAccess(typeHash, nameHash, targetIdx, target, deviceProperty, batchMode);

        return newNode;
    }

    public override INodeExpression? VisitWhileStatement([NotNull] WhileStatementContext context)
    {
        var innerCode = (IParseTree)context.block() ?? context.statement();
        var expression = Visit(context.expression())!;

        var newNode = new While(expression);
        AddToStatements(newNode);
        CurrentNode = newNode;
        Visit(innerCode);
        CurrentNode = newNode.Parent!;

        return null;
    }

    public override INodeExpression? VisitForStatement([NotNull] ForStatementContext context)
    {
        var newNode = new For();
        AddToStatements(newNode);

        CurrentNode = newNode;

        var innerCode = (IParseTree)context.block() ?? context.innerStatement;

        if (context.statement1 is not null)
        {
            Visit(context.statement1);
            newNode.HasStatement1 = true;
        }

        if (context.expression() is not null)
        {
            var expression = Visit(context.expression())!;
            newNode.Expression = expression;
        }
        else
        {
            newNode.Expression = new Literal(1);
        }

        Visit(innerCode);

        if (context.statement2 is not null)
        {
            Visit(context.statement2);
            newNode.HasStatement2 = true;
        }

        CurrentNode = newNode.Parent!;

        return null;
    }

    public override INodeExpression? VisitAssignment([NotNull] AssignmentContext context)
    {
        var expression = Visit(context.expression())!;
        var variableName = context.IDENTIFIER().GetText();

        var newNode = new VariableAssignment(variableName, expression);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression VisitNullaryOp([NotNull] NullaryOpContext context)
    {
        return new NullaryOperation(context.op.Text);
    }

    public override INodeExpression VisitUnaryOp([NotNull] UnaryOpContext context)
    {
        var operand = Visit(context.operand)!;

        var newNode = new UnaryOperation(operand, context.op.Text);
        return newNode;
    }

    public override INodeExpression VisitBinaryOp([NotNull] BinaryOpContext context)
    {
        var operand1 = Visit(context.left)!;
        var operand2 = Visit(context.right)!;

        var newNode = new BinaryOperation(operand1, operand2, context.op.Text);

        return newNode;
    }

    public override INodeExpression VisitTernaryOp([NotNull] TernaryOpContext context)
    {
        var operandA = Visit(context.a)!;
        var operandB = Visit(context.b)!;
        var operandC = Visit(context.c)!;

        var newNode = new TernaryOperation(operandA, operandB, operandC, context.op.Text);

        return newNode;
    }

    public override INodeExpression VisitIdentifier([NotNull] IdentifierContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var newNode = new UserDefinedValueAccess(name);

        return newNode;
    }

    public override INodeExpression? VisitDeviceWithIdAssignment([NotNull] DeviceWithIdAssignmentContext context)
    {
        var deviceIdxExpr = Visit(context.deviceIdxExpr)!;
        var value = Visit(context.valueExpr)!;

        var deviceProperty = context.member.Text;

        var newNode = new DeviceWithIndexAssignment(deviceIdxExpr, DeviceIndexType.Id, value, deviceProperty);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitDeviceWithIdExtendedAssignment([NotNull] DeviceWithIdExtendedAssignmentContext context)
    {
        var deviceIdxExpr = Visit(context.deviceIdxExpr)!;
        var targetIdxExpr = Visit(context.targetIdxExpr)!;
        var value = Visit(context.valueExpr)!;

        var deviceProperty = context.member?.Text;

        var newNode = new DeviceWithIndexAssignment(deviceIdxExpr, DeviceIndexType.Id, targetIdxExpr, value,
            GetDeviceTarget(context.prop.Type), deviceProperty);

        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitBatchAssignment([NotNull] BatchAssignmentContext context)
    {
        var deviceTypeHash = Visit(context.deviceTypeHashExpr)!;

        var deviceNameHash = context.deviceNameHashExpr is null
            ? null
            : Visit(context.deviceNameHashExpr)!;

        var targetIdx = context.targetIdxExpr is null
            ? null
            : Visit(context.targetIdxExpr)!;

        var value = Visit(context.valueExpr)!;

        var deviceProperty = context.member.Text;

        var target = context.prop is null
            ? DeviceTarget.Device
            : GetDeviceTarget(context.prop.Type);

        var newNode = new BatchAssignment(deviceTypeHash, deviceNameHash, targetIdx, value, deviceProperty, target);

        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitDeviceWithIndexAssignment([NotNull] DeviceWithIndexAssignmentContext context)
    {
        var deviceIdxExpr = Visit(context.deviceIdxExpr)!;
        var value = Visit(context.valueExpr)!;

        var deviceProperty = context.member.Text;

        var newNode = new DeviceWithIndexAssignment(deviceIdxExpr, DeviceIndexType.Pin, value, deviceProperty);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitDeviceWithIndexExtendedAssignment([NotNull] DeviceWithIndexExtendedAssignmentContext context)
    {
        var deviceIdxExpr = Visit(context.deviceIdxExpr)!;
        var targetIdxExpr = Visit(context.targetIdxExpr)!;
        var valueExpr = Visit(context.valueExpr)!;

        var deviceProperty = context.member?.Text;

        var newNode = new DeviceWithIndexAssignment(deviceIdxExpr, DeviceIndexType.Pin, targetIdxExpr, valueExpr, GetDeviceTarget(context.prop.Type), deviceProperty);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitDeviceIndexAccess([NotNull] DeviceIndexAccessContext context)
    {
        var member = context.member.Text;
        var deviceIdxExpr = Visit(context.deviceIdxExpr)!;

        var newNode = new DeviceWithIndexAccess(deviceIdxExpr, DeviceIndexType.Pin, member);

        return newNode;
    }

    public override INodeExpression? VisitExtendedDeviceIndexAccess([NotNull] ExtendedDeviceIndexAccessContext context)
    {
        var member = context.member?.Text;
        var deviceIdxExpr = Visit(context.deviceIdxExpr)!;
        var targetIdxExpr = Visit(context.targetIdxExpr)!;

        var newNode = new DeviceWithIndexAccess(deviceIdxExpr, DeviceIndexType.Pin, targetIdxExpr, GetDeviceTarget(context.prop.Type), member);

        return newNode;
    }

    public override INodeExpression VisitDeviceIdAccess([NotNull] DeviceIdAccessContext context)
    {
        var member = context.member.Text;

        var deviceIdExpr = Visit(context.expression())!;

        var newNode = new DeviceWithIndexAccess(deviceIdExpr, DeviceIndexType.Id, member);

        return newNode;
    }

    public override INodeExpression VisitExtendedDeviceIdAccess([NotNull] ExtendedDeviceIdAccessContext context)
    {
        var member = context.member?.Text;
        var deviceIdxExpr = Visit(context.deviceIdxExpr)!;
        var targetIdxExpr = Visit(context.targetIdxExpr)!;

        var newNode = new DeviceWithIndexAccess(deviceIdxExpr, DeviceIndexType.Id, targetIdxExpr, GetDeviceTarget(context.prop.Type), member);

        return newNode;
    }

    public override INodeExpression? VisitContinueStatement([NotNull] ContinueStatementContext context)
    {
        var newNode = new Continue();
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitBreakStatement([NotNull] BreakStatementContext context)
    {
        var newNode = new Break();
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression VisitFunctionCall([NotNull] FunctionCallContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var paramExpressions = context.expression()
            .Select(e => Visit(e)!)
            .ToList();

        var newNode = new MethodCall(name, paramExpressions);

        return newNode;
    }

    public override INodeExpression? VisitFunctionCallStatement([NotNull] FunctionCallStatementContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var paramExpressions = context.expression()
            .Select(e => Visit(e)!)
            .ToList();

        var newNode = new MethodCall(name, paramExpressions);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitReturnStatement([NotNull] ReturnStatementContext context)
    {
        var newNode = new Return();
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitReturnValueStatement([NotNull] ReturnValueStatementContext context)
    {
        var expression = Visit(context.expression())!;

        var newNode = new Return(expression);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitArraySizeDeclaration([NotNull] ArraySizeDeclarationContext context)
    {
        var sizeExpression = Visit(context.sizeExpr)!;

        var newNode = new ArrayDeclaration(context.IDENTIFIER().GetText(), sizeExpression);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitArrayListDeclaration([NotNull] ArrayListDeclarationContext context)
    {
        var elementExpressions = context.expression()
            .Select(ec => Visit(ec)!)
            .ToList();

        var newNode = new ArrayDeclaration(context.IDENTIFIER().GetText(), elementExpressions);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitArrayAssignment([NotNull] ArrayAssignmentContext context)
    {
        var indexExpr = Visit(context.indexExpr)!;
        var valueExpr = Visit(context.valueExpr)!;

        var newNode = new ArrayAssignment(context.IDENTIFIER().GetText(), indexExpr, valueExpr);
        AddToStatements(newNode);

        return null;
    }

    public override INodeExpression? VisitArrayElementAccess([NotNull] ArrayElementAccessContext context)
    {
        var indexExpr = Visit(context.indexExpr)!;
        var newNode = new ArrayAccess(context.IDENTIFIER().GetText(), indexExpr);

        return newNode;
    }

    public override INodeExpression VisitParenthesis([NotNull] ParenthesisContext context) =>
        Visit(context.expression())!;
    public override INodeExpression? VisitChildren(IRuleNode node) => base.VisitChildren(node);
    public override INodeExpression? VisitDelimitedStatement([NotNull] DelimitedStatementContext context) => base.VisitDelimitedStatement(context);
    public override INodeExpression? VisitErrorNode(IErrorNode node) => base.VisitErrorNode(node);
    public override INodeExpression? VisitStatement([NotNull] StatementContext context) => base.VisitStatement(context);
    public override INodeExpression? VisitTerminal(ITerminalNode node) => base.VisitTerminal(node);
    public override INodeExpression? VisitUndelimitedStatement([NotNull] UndelimitedStatementContext context) => base.VisitUndelimitedStatement(context);
    public override INodeExpression? VisitBlock([NotNull] BlockContext context) => base.VisitBlock(context);
    protected override INodeExpression? AggregateResult(INodeExpression? aggregate, INodeExpression? nextResult) => base.AggregateResult(aggregate, nextResult);

    private static DeviceTarget GetDeviceTarget(int propType) =>
        propType switch
        {
            SLOTS => DeviceTarget.Slots,
            REAGENTS => DeviceTarget.Reagents,
            STACK => DeviceTarget.Stack,
            _ => throw new Exception("Unecpected device target"),
        };
}
