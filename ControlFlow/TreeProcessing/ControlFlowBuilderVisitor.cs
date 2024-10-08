﻿using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;
using System.Globalization;
using static Ic11Parser;

namespace ic11.ControlFlow.TreeProcessing;
public class ControlFlowBuilderVisitor : Ic11BaseVisitor<Node?>
{
    public FlowContext FlowContext;

    private Node CurrentNode
    {
        get { return FlowContext.CurrentNode; }
        set { FlowContext.CurrentNode = value; }
    }

    private void AddToStatements(IStatement node)
    {
        if (CurrentNode is not IStatementsContainer statementsContainer)
            throw new Exception($"Unexpected statement {node?.GetType().Name}");

        ((IStatementsContainer)CurrentNode).AddToStatements(node);
    }

    public ControlFlowBuilderVisitor(FlowContext flowContext)
    {
        FlowContext = flowContext;
    }

    public override Node? Visit(IParseTree tree) => base.Visit(tree);

    public override Node? VisitDeclaration([NotNull] DeclarationContext context)
    {
        if (CurrentNode is not Root root)
            throw new Exception($"Pin declaration must be top level statement");

        var newNode = new PinDeclaration(context.IDENTIFIER().GetText(), context.PINID().GetText());
        root.Statements.Add(newNode);

        return null;
    }

    public override Node? VisitFunction([NotNull] FunctionContext context)
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

    public override Node? VisitYieldStatement([NotNull] YieldStatementContext context)
    {
        var newNode = new Yield();

        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitVariableDeclaration([NotNull] VariableDeclarationContext context)
    {
        var variableName = context.IDENTIFIER().GetText();
        var expression = (IExpression)Visit(context.expression())!;
        var newNode = new VariableDeclaration(variableName, expression);

        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitConstantDeclaration([NotNull] ConstantDeclarationContext context)
    {
        var constantName = context.IDENTIFIER().GetText();
        var expression = (IExpression)Visit(context.expression())!;
        var newNode = new ConstantDeclaration(constantName, expression);

        AddToStatements(newNode);

        return null;
    }

    public override Node VisitLiteral([NotNull] LiteralContext context)
    {
        var value = context.GetText();

        if (value == "true")
            value = "1";

        if (value == "false")
            value = "0";

        var parsedValue = decimal.Parse(value, CultureInfo.InvariantCulture);

        return new Literal(parsedValue);
    }

    public override Node? VisitIfStatement([NotNull] IfStatementContext context)
    {
        var hasElsePart = context.ELSE() is not null;

        var blocks = context.block();
        var rawStatements = context.statement();

        List<IParseTree> codeContents = blocks.Any()
            ? blocks.Select(b => (IParseTree)b).ToList()
            : rawStatements.Select(s => (IParseTree)s).ToList();

        if (hasElsePart && blocks.Length == 1 && rawStatements.Length == 1)
            throw new Exception($"Inconsistent if-else satement. Either use blocks or raw statements for both parts.");

        var expression = (IExpression)Visit(context.expression())!;

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

    public override Node? VisitMemberAssignment([NotNull] MemberAssignmentContext context)
    {
        var valueExpr = (IExpression)Visit(context.valueExpr)!;

        var member = context.member.Text;
        var device = context.identifier.Text;

        if (context.identifier.Type == BASE_DEVICE)
            device = "db";

        var newNode = new MemberAssignment(device, member, valueExpr);
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitMemberSlotAssignment([NotNull] MemberSlotAssignmentContext context)
    {
        var valueExpr = (IExpression)Visit(context.valueExpr)!;
        var slotIdxExpr = (IExpression)Visit(context.slotIdxExpr)!;

        var member = context.member.Text;
        var device = context.identifier.Text;

        if (context.identifier.Type == BASE_DEVICE)
            device = "db";

        var newNode = new MemberAssignment(device, member, slotIdxExpr, valueExpr);
        AddToStatements(newNode);

        return null;
    }

    public override Node VisitMemberAccess([NotNull] MemberAccessContext context)
    {
        var member = context.member.Text;
        var device = context.identifier.Text;

        if (context.identifier.Type == BASE_DEVICE)
            device = "db";

        var newNode = new MemberAccess(device, member);

        return newNode;
    }

    public override Node? VisitSlotMemberAccess([NotNull] SlotMemberAccessContext context)
    {
        var slotIdxExpr = (IExpression)Visit(context.slotIdxExpr)!;

        var member = context.member.Text;
        var device = context.identifier.Text;

        if (context.identifier.Type == BASE_DEVICE)
            device = "db";

        var newNode = new MemberAccess(device, slotIdxExpr, member);

        return newNode;
    }

    public override Node? VisitWhileStatement([NotNull] WhileStatementContext context)
    {
        var innerCode = (IParseTree)context.block() ?? context.statement();
        var expression = (IExpression)Visit(context.expression())!;

        var newNode = new While(expression);
        AddToStatements(newNode);
        CurrentNode = newNode;
        Visit(innerCode);
        CurrentNode = newNode.Parent;

        return null;
    }

    public override Node? VisitAssignment([NotNull] AssignmentContext context)
    {
        var expression = (IExpression)Visit(context.expression())!;
        var variableName = context.IDENTIFIER().GetText();

        var newNode = new VariableAssignment(variableName, expression);
        AddToStatements(newNode);

        return null;
    }

    public override Node VisitBinaryOp([NotNull] BinaryOpContext context)
    {
        var operand1 = (IExpression)Visit(context.left)!;
        var operand2 = (IExpression)Visit(context.right)!;

        var type = context.op.Type switch
        {
            ADD => BinaryOperationType.Add,
            SUB => BinaryOperationType.Sub,
            MUL => BinaryOperationType.Mul,
            DIV => BinaryOperationType.Div,
            AND => BinaryOperationType.And,
            OR => BinaryOperationType.Or,
            EQ => BinaryOperationType.Eq,
            NE => BinaryOperationType.Ne,
            LE => BinaryOperationType.Le,
            LT => BinaryOperationType.Lt,
            GE => BinaryOperationType.Ge,
            GT => BinaryOperationType.GT,

            _ => throw new NotImplementedException(),
        };

        var newNode = new BinaryOperation(operand1, operand2, type);

        return newNode;
    }

    public override Node VisitUnaryOp([NotNull] UnaryOpContext context)
    {
        var operand = (IExpression)Visit(context.operand)!;

        var type = context.op.Type switch
        {
            NEGATION => UnaryOperationType.Not,
            SUB => UnaryOperationType.Minus,
            ABS => UnaryOperationType.Abs,
            _ => throw new NotImplementedException(),
        };

        var newNode = new UnaryOperation(operand, type);
        return newNode;
    }

    public override Node VisitIdentifier([NotNull] IdentifierContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var newNode = new UserDefinedValueAccess(name);

        return newNode;
    }

    public override Node? VisitDeviceWithIdAssignment([NotNull] DeviceWithIdAssignmentContext context)
    {
        var idValue = (IExpression)Visit(context.idExpr)!;
        var value = (IExpression)Visit(context.valueExpr)!;

        var deviceProperty = context.IDENTIFIER().GetText();

        var newNode = new DeviceWithIdAssignment(idValue, value, deviceProperty);
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitDeviceWithIndexAssignment([NotNull] DeviceWithIndexAssignmentContext context)
    {
        var index = (IExpression)Visit(context.pinIdxExpr)!;
        var value = (IExpression)Visit(context.valueExpr)!;

        var deviceProperty = context.member.Text;

        var newNode = new DeviceWithIndexAssignment(index, value, deviceProperty);
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitDeviceWithIndexSlotAssignment([NotNull] DeviceWithIndexSlotAssignmentContext context)
    {
        var pinIndexExpr = (IExpression)Visit(context.pinIdxExpr)!;
        var slotIndexExpr = (IExpression)Visit(context.slotIdxExpr)!;
        var valueExpr = (IExpression)Visit(context.valueExpr)!;

        var deviceProperty = context.member.Text;

        var newNode = new DeviceWithIndexAssignment(pinIndexExpr, slotIndexExpr, valueExpr, deviceProperty);
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitDeviceIndexAccess([NotNull] DeviceIndexAccessContext context)
    {
        var member = context.member.Text;
        var indexValue = (IExpression)Visit(context.pinIdxExpr)!;

        var newNode = new DeviceWithIndexAccess(indexValue, member);

        return newNode;
    }

    public override Node? VisitSlotDeviceIndexAccess([NotNull] SlotDeviceIndexAccessContext context)
    {
        var member = context.member.Text;
        var pinIdxExpr = (IExpression)Visit(context.pinIdxExpr)!;
        var slotIdxExpr = (IExpression)Visit(context.slotIdxExpr)!;

        var newNode = new DeviceWithIndexAccess(pinIdxExpr, slotIdxExpr, member);

        return newNode;
    }

    public override Node VisitDeviceIdAccess([NotNull] DeviceIdAccessContext context)
    {
        var deviceProperty = context.IDENTIFIER().GetText();

        var deviceIdExpr = (IExpression)Visit(context.expression())!;

        var newNode = new DeviceWithIdAccess(deviceIdExpr, deviceProperty);

        return newNode;
    }

    public override Node? VisitContinueStatement([NotNull] ContinueStatementContext context)
    {
        var newNode = new Continue();
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitBreakStatement([NotNull] BreakStatementContext context)
    {
        var newNode = new Break();
        AddToStatements(newNode);

        return null;
    }

    public override Node VisitFunctionCall([NotNull] FunctionCallContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var paramExpressions = context.expression()
            .Select(e => (IExpression)Visit(e)!)
            .ToList();

        var newNode = new MethodCall(name, paramExpressions);

        return newNode;
    }

    public override Node? VisitFunctionCallStatement([NotNull] FunctionCallStatementContext context)
    {
        var name = context.IDENTIFIER().GetText();

        var paramExpressions = context.expression()
            .Select(e => (IExpression)Visit(e)!)
            .ToList();

        var newNode = new MethodCall(name, paramExpressions);
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitReturnStatement([NotNull] ReturnStatementContext context)
    {
        var newNode = new Return();
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitReturnValueStatement([NotNull] ReturnValueStatementContext context)
    {
        var expression = (IExpression)Visit(context.expression())!;

        var newNode = new Return(expression);
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitArraySizeDeclaration([NotNull] ArraySizeDeclarationContext context)
    {
        var sizeExpression = (IExpression)Visit(context.sizeExpr)!;

        var newNode = new ArrayDeclaration(context.IDENTIFIER().GetText(), sizeExpression);
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitArrayListDeclaration([NotNull] ArrayListDeclarationContext context)
    {
        var elementExpressions = context.expression()
            .Select(ec => (IExpression)Visit(ec)!)
            .ToList();

        var newNode = new ArrayDeclaration(context.IDENTIFIER().GetText(), elementExpressions);
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitArrayAssignment([NotNull] ArrayAssignmentContext context)
    {
        var indexExpr = (IExpression)Visit(context.indexExpr)!;
        var valueExpr = (IExpression)Visit(context.valueExpr)!;

        var newNode = new ArrayAssignment(context.IDENTIFIER().GetText(), indexExpr, valueExpr);
        AddToStatements(newNode);

        return null;
    }

    public override Node? VisitArrayElementAccess([NotNull] ArrayElementAccessContext context)
    {
        var indexExpr = (IExpression)Visit(context.indexExpr)!;
        var newNode = new ArrayAccess(context.IDENTIFIER().GetText(), indexExpr);

        return newNode;
    }

    public override Node VisitParenthesis([NotNull] ParenthesisContext context) =>
        Visit(context.expression())!;
    public override Node? VisitChildren(IRuleNode node) => base.VisitChildren(node);
    public override Node? VisitDelimitedStatement([NotNull] DelimitedStatementContext context) => base.VisitDelimitedStatement(context);
    public override Node? VisitErrorNode(IErrorNode node) => base.VisitErrorNode(node);
    public override Node? VisitStatement([NotNull] StatementContext context) => base.VisitStatement(context);
    public override Node? VisitTerminal(ITerminalNode node) => base.VisitTerminal(node);
    public override Node? VisitUndelimitedStatement([NotNull] UndelimitedStatementContext context) => base.VisitUndelimitedStatement(context);
    public override Node? VisitBlock([NotNull] BlockContext context) => base.VisitBlock(context);
    protected override Node? AggregateResult(Node? aggregate, Node? nextResult) => base.AggregateResult(aggregate, nextResult);

}
