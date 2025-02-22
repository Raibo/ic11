﻿using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;
using System.Text;

namespace ic11.ControlFlow.TreeProcessing;
public class ControlFlowTreeVisualizer : ControlFlowTreeVisitorBase<object?>
{
    private StringBuilder _sb;
    private int _depth;
    private const int _indent = 4;

    protected override Type VisitorType => typeof(ControlFlowTreeVisualizer);

    public string Visualize(Node node)
    {
        _sb = new();
        _depth = 0;

        Visit(node);

        return _sb.ToString();
    }

    private string Indent() =>
        _depth > 0
            //? new string(' ', _depth * _indent - 2) + "^-"
            ? new string(' ', _depth * _indent)
            : String.Empty;

    private void WriteLine(string s) => _sb.AppendLine($"{Indent()}{s}");

    private string Tags(Node node)
    {
        var tagSb = new StringBuilder();

        if (node.Scope is not null)
            tagSb.Append($" [scope {node.Scope.Id}]");

        tagSb.Append($" [ord {node.IndexInScope}]");

        if (node is IExpression ex && ex.Variable is not null)
            tagSb.Append($" [var{ex.Variable.Id}/{ex.Variable.Register}:d{ex.Variable.DeclareIndex}:a{ex.Variable.LastReassignedIndex}:r{ex.Variable.LastReferencedIndex}]");

        if (node is VariableDeclaration d && d.Variable is not null)
            tagSb.Append($" [var{d.Variable.Id}/{d.Variable.Register}]");

        if (node is VariableAssignment a && a.Variable is not null)
            tagSb.Append($" [var{a.Variable.Id}/{a.Variable.Register}]");

        if (node.IsUnreachableCode)
            tagSb.Append(" [Unreachable]");

        if (node is MethodDeclaration dec && dec.NotAllPathsReturnValue)
            tagSb.Append(" [NotAllPathsReturnValue]");

        return tagSb.ToString();
    }

    private object? Visit(Root root)
    {
        WriteLine($"Root{Tags(root)}");
        VisitStatements(root.Statements);
        return null;
    }

    private object? Visit(PinDeclaration node)
    {
        WriteLine($"PinDeclaration (Name {node.Name}, Device {node.Device}){Tags(node)}");
        return null;
    }

    private object? Visit(MethodDeclaration node)
    {
        WriteLine($"{node.ReturnType} {node.Name}{Tags(node)}");
        VisitStatements(node.Statements);
        return null;
    }

    private object? Visit(While node)
    {
        WriteLine($"While-expr{Tags(node)}");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;

        WriteLine($"While{Tags(node)}");
        VisitStatements(node.Statements);
        return null;
    }

    private object? Visit(If node)
    {
        WriteLine($"If-expr{Tags(node)}");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;

        WriteLine($"If{Tags(node)}");
        node.CurrentStatementsContainer = DataHolders.IfStatementsContainer.If;
        VisitStatements(node.Statements);

        if (node.ElseStatements.Any())
        {
            WriteLine($"Else{Tags(node)}");
            node.CurrentStatementsContainer = DataHolders.IfStatementsContainer.Else;
            VisitStatements(node.Statements);
        }

        return null;
    }

    private object? VisitStatements(List<IStatement> statements)
    {
        _depth++;

        foreach (var item in statements)
            Visit((Node)item);

        _depth--;
        return null;
    }

    private object? Visit(StatementParam0 node)
    {
        WriteLine($"{node.Operation}{Tags(node)}");
        return null;
    }

    private object? Visit(StatementParam1 node)
    {
        WriteLine($"{node.Operation}{Tags(node)}");
        _depth++;
        Visit((Node)node.Parameter);
        _depth--;
        return null;
    }

    private object? Visit(VariableDeclaration node)
    {
        WriteLine($"Var declaration {node.Name} = ?{Tags(node)}");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;
        return null;
    }

    private object? Visit(ConstantDeclaration node)
    {
        WriteLine($"Const declaration {node.Name} = {node.Expression.CtKnownValue}{Tags(node)}");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;
        return null;
    }

    private object? Visit(UserDefinedValueAccess node)
    {
        WriteLine($"Var access {node.Name}{Tags(node)}");
        return null;
    }

    private object? Visit(VariableAssignment node)
    {
        WriteLine($"Var assignment {node.Name} = ?{Tags(node)}");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;
        return null;
    }

    private object? Visit(Literal node)
    {
        WriteLine($"Literal {node.CtKnownValue}{Tags(node)}");
        return null;
    }

    private object? Visit(MemberAssignment node)
    {
        WriteLine($"Device assignment {node.Name}.{node.MemberName} = ?{Tags(node)}");
        _depth++;
        Visit((Node)node.ValueExpression);
        _depth--;
        return null;
    }

    private object? Visit(NullaryOperation node)
    {
        WriteLine($"Nullary operation {node.Operation}{Tags(node)}");
        return null;
    }

    private object? Visit(UnaryOperation node)
    {
        WriteLine($"Unary operation {node.Operation}{Tags(node)}");
        _depth++;
        Visit((Node)node.Operand);
        _depth--;
        return null;
    }

    private object? Visit(BinaryOperation node)
    {
        WriteLine($"Binary operation {node.Operation}{Tags(node)}");
        _depth++;
        Visit((Node)node.Left);
        Visit((Node)node.Right);
        _depth--;
        return null;
    }

    private object? Visit(TernaryOperation node)
    {
        WriteLine($"Binary operation {node.Operation}{Tags(node)}");
        _depth++;
        Visit((Node)node.OperandA);
        Visit((Node)node.OperandB);
        Visit((Node)node.OperandC);
        _depth--;
        return null;
    }

    private object? Visit(MemberAccess node)
    {
        WriteLine($"Member access {node.Name}.{node.MemberName}{Tags(node)}");
        return null;
    }

    private object? Visit(MethodCall node)
    {
        WriteLine($"Method call {node.Name}(. . .){Tags(node)}");

        _depth++;
        foreach (var item in node.ArgumentExpressions)
            Visit((Node)item);
        _depth--;
        return null;
    }

    private object? Visit(Return node)
    {
        if (!node.HasValue)
        {
            WriteLine($"Return{Tags(node)}");
            return null;
        }

        WriteLine($"Return value{Tags(node)}");
        _depth++;
        Visit((Node)node.Expression!);
        _depth--;
        return null;
    }

    private object? Visit(Continue node)
    {
        WriteLine($"Continue{Tags(node)}");
        return null;
    }

    private object? Visit(Break node)
    {
        WriteLine($"Break{Tags(node)}");
        return null;
    }

    private object? Visit(DeviceWithIndexAccess node)
    {
        WriteLine($"Device with index access (member {node.MemberName}){Tags(node)}");
        _depth++;
        Visit((Node)node.DeviceIndexExpr);
        _depth--;
        return null;
    }

    private object? Visit(ArrayDeclaration node)
    {
        WriteLine($"Array decl{Tags(node)}");
        _depth++;

        foreach (Node item in node.Expressions)
            Visit(item);

        _depth--;
        return null;
    }

    private object? Visit(ArrayAssignment node)
    {
        WriteLine($"Array assignment{Tags(node)}");
        _depth++;

        foreach (Node item in node.Expressions)
            Visit(item);

        _depth--;
        return null;
    }

    private object? Visit(ArrayAccess node)
    {
        WriteLine($"Array access{Tags(node)}");
        _depth++;

        foreach (Node item in node.Expressions)
            Visit(item);

        _depth--;
        return null;
    }
}
