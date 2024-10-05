namespace ic11.ControlFlow.Context;
public class Variable
{
    public readonly int Id = _staticId++;

    private static int _staticId = 0;

    public Scope DeclareScope;
    public int DeclareIndex;
    public int LastReferencedIndex = -1;
    public int LastReassignedIndex = -1;

    public override string ToString() => $"{{Variable{Id} dec={DeclareIndex} ref={LastReferencedIndex} reg={Register} }}";
    public string Register;

    //private string _reg;
    //public string Register { get => _reg is null ? null : $"var{Id}[{_reg}] dec[{DeclareIndex}] ref[{LastReferencedIndex}]"; set { _reg = value; } }

    public override bool Equals(object? other) =>
        other is Variable otherV && otherV.Id == Id;

    public override int GetHashCode() => Id;
}
