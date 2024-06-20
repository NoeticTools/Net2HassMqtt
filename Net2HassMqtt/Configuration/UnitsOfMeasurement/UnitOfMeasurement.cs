namespace NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;

public abstract class UnitOfMeasurement : IEquatable<UnitOfMeasurement>
{
    protected UnitOfMeasurement(string hassUnitOfMeasurement)
    {
        HassUnitOfMeasurement = hassUnitOfMeasurement;
    }

    public string HassUnitOfMeasurement { get; }

    public bool Equals(UnitOfMeasurement? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return HassUnitOfMeasurement == other.HassUnitOfMeasurement;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((UnitOfMeasurement)obj);
    }

    public override int GetHashCode()
    {
        return HassUnitOfMeasurement.GetHashCode();
    }
}