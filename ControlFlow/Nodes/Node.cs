namespace ic11.ControlFlow.Nodes;
public abstract class Node
{
    public Node? Parent;
    public readonly int Id;

    public bool IsUnreachableCode = false;

    private static int NextNodeId;

    public Node()
    {
        Id = NextNodeId++;
    }

    public override bool Equals(object? obj) =>
        obj is Node other && other.Id == Id;

    public bool Equals(Node? other) =>
        other is not null && other.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(Node a, Node b) =>
        a.Equals(b);

    public static bool operator !=(Node a, Node b) =>
        !a.Equals(b);
}
