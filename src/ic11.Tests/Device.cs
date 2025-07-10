namespace ic11.Tests;

public class Device
{
    public readonly Dictionary<string, double> Properties = new();
    public void SetProperty(string propertyName, double value)
    {
        Properties[propertyName] = value;
    }
    
    public double GetProperty(string propertyName)
    {
        if (Properties.TryGetValue(propertyName, out var value))
        {
            return value;
        }

        return 0;
    }
}