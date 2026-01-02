using ic11.ControlFlow.Context;

namespace ic11.ControlFlow.NodeInterfaces;

public interface INode
{
    INode? Parent { get; set; }
    int Id { get; }
    Scope? Scope { get; set; }
    int IndexSize { get; }
    int IndexInScope { get; set; }
    bool IsUnreachableCode { get; set; }

    bool Equals(object? obj);
    bool Equals(INode? other);
    int GetHashCode();
    void SetIndex(ref int index);
}