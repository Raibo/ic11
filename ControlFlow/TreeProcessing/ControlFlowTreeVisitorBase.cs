using ic11.ControlFlow.Nodes;
using System.Reflection;

namespace ic11.ControlFlow.TreeProcessing;
public abstract class ControlFlowTreeVisitorBase<TResult>
{
    public bool AllowMethodSkip = false;

    private readonly Dictionary<Type, MethodInfo> _methodsMap = new();
    protected abstract Type VisitorType { get; }

    public virtual TResult Visit(Node node)
    {
        var nodeType = node.GetType();
        var methodInfo = GetVisitMethodForNodeType(nodeType);

        return methodInfo is null
            ? default!
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
            .SingleOrDefault();

        if (preciseMethod is not null)
            return preciseMethod;

        if (!AllowMethodSkip)
            throw new Exception($"No {nameof(Visit)} method for node type {nodeType.Name}");

        return null;
    }
}
