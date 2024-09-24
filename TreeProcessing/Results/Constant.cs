namespace ic11.TreeProcessing.Results
{
    public class Constant : IValue
    {
        public Constant(double value)
        {
            Value = value;
        }

        public double Value;

        public override string ToString() => "CONST " + Value;
    }
}