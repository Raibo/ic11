namespace ic11.TreeProcessing.Results
{
    public class Variable : IValue
    {
        public Variable(string name)
        {
            Name = name;
        }

        public string Name;

        public override string ToString() => "VAR " + Name;
    }
}