using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.TreeProcessing;
public class RegisterVisitor
{
    private readonly FlowContext _flowContext;
    private HashSet<string> _currentUsedRegisters;
    private MethodDeclaration _currentMethod;

    public RegisterVisitor(FlowContext flowContext)
    {
        _flowContext = flowContext;
    }

    public void DoWork()
    {
        var methodDeclarations = _flowContext.DeclaredMethods;

        foreach (var item in methodDeclarations.Values)
            VisitNode(item);
    }

    private void VisitNode(MethodDeclaration methodDeclaration)
    {
        _currentMethod = methodDeclaration;
        _currentUsedRegisters = new HashSet<string>();

        // Method parameters must have registers, even if unused. At least to have somewhere to pop into from stack
        foreach (var pv in methodDeclaration.ParameterVariables)
        {
            var register = methodDeclaration.InnerScope!.GetAvailableRegister(pv.DeclareIndex, pv.LastReferencedIndex);
            AssignRegister(pv, register);
        }

        foreach (var node in methodDeclaration.Statements)
            VisitNode((Node)node);

        methodDeclaration.UsedRegistersCount = _currentUsedRegisters.Count();
    }

    private void AssignRegister(Variable var, string register)
    {
        var.Register = register;
        _currentMethod.AllVariables.Add(var);

        if (var.LastReferencedIndex >= 0 || var.LastReassignedIndex >= 0)
            _currentUsedRegisters.Add(register);
    }

    private void VisitNode(Node node)
    {
        // Arrays need their reference value be saved before elements or size, because later sp register may change
        if (node is ArrayDeclaration ad && ad.AddressVariable is not null && ad.AddressVariable.Register is null)
        {
            var register = node.Scope!.GetAvailableRegister(ad.AddressVariable.DeclareIndex, ad.AddressVariable.LastReferencedIndex);
            AssignRegister(ad.AddressVariable, register);
        }

        if (node is IExpressionContainer ec)
        {
            foreach (Node item in ec.Expressions)
                VisitNode(item);
        }

        if (node is IExpression ex && ex.Variable is not null && ex.Variable.Register is null)
        {
            var register = node.Scope!.GetAvailableRegister(ex.Variable.DeclareIndex, ex.Variable.LastReferencedIndex);
            AssignRegister(ex.Variable, register);
        }

        if (node is VariableDeclaration d && d.Variable is not null && d.Variable.Register is null)
        {
            var register = node.Scope!.GetAvailableRegister(d.Variable.DeclareIndex, d.Variable.LastReferencedIndex);
            AssignRegister(d.Variable, register);
        }

        if (node is ArrayAssignment ass && ass.Variable is not null && ass.Variable.Register is null)
        {
            var register = node.Scope!.GetAvailableRegister(ass.Variable.DeclareIndex, ass.Variable.LastReferencedIndex);
            AssignRegister(ass.Variable, register);
        }

        if (node is ArrayAccess aa && aa.Variable is not null && aa.Variable.Register is null)
        {
            var register = node.Scope!.GetAvailableRegister(aa.Variable.DeclareIndex, aa.Variable.LastReferencedIndex);
            AssignRegister(aa.Variable, register);
        }

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
    }
}
