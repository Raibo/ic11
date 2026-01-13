using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;
using System.Reflection;

namespace ic11.ControlFlow.TreeProcessing;
public abstract class ControlFlowTreeVisitorBase<TResult>
{
    public bool AllowMethodSkip = false;
    public TResult SkippedReturnValue = default!;

    private readonly Dictionary<Type, MethodInfo> _methodsMap = new();
    protected abstract Type VisitorType { get; }

    public virtual TResult Visit(INode node)
    {
        var nodeType = node.GetType();
        var methodInfo = GetVisitMethodForNodeType(nodeType);

        return methodInfo is null
            ? SkippedReturnValue
            : (TResult)methodInfo.Invoke(this, new[] { node })!;
    }

    private MethodInfo? GetVisitMethodForNodeType(Type nodeType)
    {
        if (_methodsMap.TryGetValue(nodeType, out var methodInfo))
            return methodInfo;

        var preciseMethod = VisitorType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.Name == nameof(Visit))
            .Where(m => m.ReturnParameter.ParameterType == typeof(TResult))
            .Where(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == nodeType)
            .OrderByDescending(m => InheritanceCount(m.DeclaringType))
            .FirstOrDefault();

        if (preciseMethod is not null)
            return preciseMethod;

        if (!AllowMethodSkip)
            throw new Exception($"No {nameof(Visit)} method for node type {nodeType.Name}");

        return null;
    }

    private int InheritanceCount(Type? type)
    {
        if (type is null)
            return 0;

        var currentType = type;

        var count = 0;
        while (currentType != typeof(object))
        {
            currentType = currentType!.BaseType;
            count++;
        }

        return count;
    }

    protected virtual TResult Visit(Root node)
    {
        foreach (INode item in node.Statements)
            Visit(item);

        return default!;
    }

    protected virtual TResult Visit(MethodDeclaration node)
    {
        foreach (INode item in node.Statements)
            Visit(item);

        return default!;
    }

    protected virtual TResult Visit(While node)
    {
        foreach (INode item in node.Statements)
            Visit(item);

        return default!;
    }

    protected virtual TResult Visit(For node)
    {
        foreach (INode item in node.Statements)
            Visit(item);

        return default!;
    }

    protected virtual TResult Visit(If node)
    {
        node.CurrentStatementsContainer = IfStatementsContainer.If;

        foreach (INode item in node.Statements)
            Visit(item);

        node.CurrentStatementsContainer = IfStatementsContainer.Else;

        foreach (INode item in node.Statements)
            Visit(item);

        return default!;
    }
}
