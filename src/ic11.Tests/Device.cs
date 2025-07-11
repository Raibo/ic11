namespace ic11.Tests;

public class Device
{
    public readonly Dictionary<string, double> GeneralProperties = new();
    public readonly Dictionary<int, Dictionary<string, double>> SlotProperties = new();
    public double[] Stack = null!;
    
    public void SetProperty(string propertyName, double value)
    {
        GeneralProperties[propertyName] = value;
    }
    
    public double GetProperty(string propertyName)
    {
        if (GeneralProperties.TryGetValue(propertyName, out var value))
        {
            return value;
        }

        return 0;
    }
    
    public void SetSlotProperty(double slot, string propertyName, double value)
    {
        if (!SlotProperties.TryGetValue((int)slot, out var properties))
        {
            properties = new Dictionary<string, double>();
            SlotProperties[(int)slot] = properties;
        }

        properties[propertyName] = value;
    }

    public double GetSlotProperty(int slot, string propertyName)
    {
        if (SlotProperties.TryGetValue(slot, out var properties))
        {
            if (properties.TryGetValue(propertyName, out var value))
            {
                return value;
            }
        }

        return 0;
    }
}