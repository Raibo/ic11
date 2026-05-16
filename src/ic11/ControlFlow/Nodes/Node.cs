using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public abstract class Node : INode
{
    public INode? Parent { get; set; }
    public int Id { get; }
    public Scope? Scope { get; set; }
    public int IndexInScope { get; set; }

    public virtual int IndexSize => 1;

    public bool IsUnreachableCode { get; set; } = false;

    private static int NextNodeId;

    public Node()
    {
        Id = NextNodeId++;
    }

    public void SetIndex(ref int index)
    {
        IndexInScope = index;
        index += IndexSize;
    }

    public override bool Equals(object? obj) =>
        obj is Node other && other.Id == Id;

    public bool Equals(INode? other) =>
        other is not null && other.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(Node a, Node b) =>
        a.Equals(b);

    public static bool operator !=(Node a, Node b) =>
        !a.Equals(b);
}
