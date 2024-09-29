using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;
using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Instructions;
using ic11.TreeProcessing.Results;

namespace ic11.ControlFlow.TreeProcessing;
public class Ic10CommandGenerator : ControlFlowTreeVisitorBase<IValue?>
{
    protected override Type VisitorType => typeof(Ic10CommandGenerator);

    public readonly CompileContext CompileContext;
    /*
    private Scope CurrentScope => CompileContext.CurrentScope;

    public Ic10CommandGenerator(CompileContext compileContext)
    {
        CompileContext = compileContext;
    }

    private object? Visit(Root root)
    {
        VisitStatements(root.Statements);
        return null;
    }

    private object? Visit(PinDeclaration node)
    {
        
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

    private object? Visit(Yield node)
    {
        WriteLine($"Yield{Tags(node)}");
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

    private object? Visit(VariableAccess node)
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
        WriteLine($"Literal {node.Value}{Tags(node)}");
        return null;
    }

    private object? Visit(MemberAssignment node)
    {
        WriteLine($"Device assignment {node.Name}.{node.MemberName} = ?{Tags(node)}");
        _depth++;
        Visit((Node)node.Expression);
        _depth--;
        return null;
    }

    private object? Visit(BinaryOperation node)
    {
        WriteLine($"Binary operation {node.Type}{Tags(node)}");
        _depth++;
        Visit((Node)node.Left);
        Visit((Node)node.Right);
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

    private object? Visit(UnaryOperation node)
    {
        WriteLine($"Unary operation {node.Type}{Tags(node)}");
        _depth++;
        Visit((Node)node.Operand);
        _depth--;
        return null;
    }

    private object? Visit(DeviceWithIdAccess node)
    {
        WriteLine($"Device with id access (member {node.Member}){Tags(node)}");
        _depth++;
        Visit((Node)node.RefIdExpr);
        _depth--;
        return null;
    }

    private object? Visit(DeviceWithIdAssignment node)
    {
        WriteLine($"Device with id assignment (member {node.Member}){Tags(node)}");
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
            WriteLine($"Return{Tags(node)}");
            return null;
        }

        WriteLine($"Return value{Tags(node)}");
        _depth++;
        Visit((Node)node.Expression!);
        _depth--;
        return null;
    }*/
}
