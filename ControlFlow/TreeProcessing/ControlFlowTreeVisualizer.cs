using ic11.ControlFlow.NodeInterfaces;
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

    private object? WriteLine(string s) => _sb.AppendLine($"{Indent()}{s}");

    private object? Visit(Root root)
    {
        WriteLine("Root");
        VisitStatements(root.Statements);
        return null;
    }

    private object? Visit(PinDeclaration node)
    {
        WriteLine($"PinDeclaration (Name {node.Name}, Device {node.Device})");
        return null;
    }

    private object? Visit(MethodDeclaration node)
    {
        WriteLine($"{node.ReturnType} {node.Name}");
        VisitStatements(node.Statements);
        return null;
    }

    private object? Visit(While node)
    {
        WriteLine($"While-expr");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;

        WriteLine("While");
        VisitStatements(node.Statements);
        return null;
    }

    private object? Visit(If node)
    {
        WriteLine($"If-expr");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;

        WriteLine($"If");
        node.CurrentStatementsContainer = DataHolders.IfStatementsContainer.If;
        VisitStatements(node.Statements);

        if (node.ElseStatements.Any())
        {
            WriteLine($"Else");
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

    private object? Visit(Yield node)
    {
        WriteLine("Yield");
        return null;
    }

    private object? Visit(VariableDeclaration node)
    {
        WriteLine($"Var declaration {node.Name} = ?");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;
        return null;
    }

    private object? Visit(VariableAccess node)
    {
        WriteLine($"Var access {node.Name}");
        return null;
    }

    private object? Visit(VariableAssignment node)
    {
        WriteLine($"Var assignment {node.Name} = ?");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;
        return null;
    }

    private object? Visit(Literal node)
    {
        WriteLine($"Literal {node.Value}");
        return null;
    }

    private object? Visit(MemberAssignment node)
    {
        WriteLine($"Device assignment {node.Name}.{node.MemberName} = ?");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;
        return null;
    }

    private object? Visit(BinaryOperation node)
    {
        WriteLine($"Binary operation {node.Type}");
        _depth++;
        Visit((Node)node.Left);
        Visit((Node)node.Right);
        _depth--;
        return null;
    }

    private object? Visit(MemberAccess node)
    {
        WriteLine($"Member access {node.Name}.{node.MemberName}");
        return null;
    }

    private object? Visit(MethodCall node)
    {
        WriteLine($"Method call {node.Name}(. . .)");

        _depth++;
        foreach (var item in node.ArgumentExpressions)
            Visit((Node)item);
        _depth--;
        return null;
    }

    private object? Visit(UnaryOperation node)
    {
        WriteLine($"Unary operation {node.Type}");
        _depth++;
        Visit((Node)node.Operand);
        _depth--;
        return null;
    }

    private object? Visit(DeviceWithIdAccess node)
    {
        WriteLine($"Device with id access (member {node.Member})");
        _depth++;
        Visit((Node)node.RefIdExpr);
        _depth--;
        return null;
    }

    private object? Visit(DeviceWithIdAssignment node)
    {
        WriteLine($"Device with id assignment (member {node.Member})");
        _depth++;
        Visit((Node)node.RefIdExpr);
        Visit((Node)node.ValueExpr);
        _depth--;
        return null;
    }

    private object? Visit(Return node)
    {
        if (!node.HasValue)
        {
            WriteLine($"Return");
            return null;
        }

        WriteLine($"Return value");
        _depth++;
        Visit((Node)node.Expression!);
        _depth--;
        return null;
    }
}
