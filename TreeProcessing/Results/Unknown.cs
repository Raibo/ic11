namespace ic11.TreeProcessing.Results
{
    public class Unknown : IValue
    {
        public Unknown(string help)
        {
            Help = help;
        }

        public string Help;

        public override string ToString() => "UNKNOWN<" + Help + ">";
    }
}