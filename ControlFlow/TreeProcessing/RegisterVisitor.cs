using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class RegisterVisitor
{
    public void Visit(Root node)
    {
        foreach (Node statement in node.Statements)
            VisitNode(statement);
    }

    private object? VisitNode(Node node)
    {
        if (node is IExpressionContainer ec)
        {
            foreach (Node item in ec.Expressions)
                VisitNode(item);
        }

        if (node is IExpression ex && ex.Variable is not null && ex.Variable.Register is null)
            ex.Variable.Register = node.Scope!.GetAvailableRegister(ex.Variable.DeclareIndex, ex.Variable.LastReferencedIndex);

        if (node is VariableDeclaration d && d.Variable is not null && d.Variable.Register is null)
            d.Variable.Register = node.Scope!.GetAvailableRegister(d.Variable.DeclareIndex, d.Variable.LastReferencedIndex);

        if (node is ArrayDeclaration ad && ad.AddressVariable is not null && ad.AddressVariable.Register is null)
            ad.AddressVariable.Register = node.Scope!.GetAvailableRegister(ad.AddressVariable.DeclareIndex, ad.AddressVariable.LastReferencedIndex);

        if (node is ArrayAssignment ass && ass.Variable is not null && ass.Variable.Register is null)
            ass.Variable.Register = node.Scope!.GetAvailableRegister(ass.Variable.DeclareIndex, ass.Variable.LastReferencedIndex);

        if (node is ArrayAccess aa && aa.Variable is not null && aa.Variable.Register is null)
            aa.Variable.Register = node.Scope!.GetAvailableRegister(aa.Variable.DeclareIndex, aa.Variable.LastReferencedIndex);

        if (node is IStatementsContainer sc && node is not If)
        {
            foreach (Node item in sc.Statements)
                VisitNode(item);
        }

        if (node is If ifNode)
        {
            foreach (Node item in ifNode.IfStatements)
                VisitNode(item);

            foreach (Node item in ifNode.ElseStatements)
                VisitNode(item);
        }

        return null;
    }
}
