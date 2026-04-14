using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

internal sealed class NumericValueConverter(ILogger logger) : ModelValueConverterBase(logger), IModelValueConverter
{
    private static readonly HashSet<string> ValueHassDomains =
    [
        HassDomains.Humidifier.HassDomainName,
        HassDomains.Sensor.HassDomainName,
        HassDomains.Number.HassDomainName
    ];

    public bool CanConvert(ModelValueDescriptor valueDescriptor)
    {
        return ValueHassDomains.Contains(valueDescriptor.HassDomainName) &&
               (
                   valueDescriptor.ModelPropertyType == typeof(int) ||
                   valueDescriptor.ModelPropertyType == typeof(double)
               );
    }

    public Func<object?, string> GetConverter(ModelValueDescriptor valueDescriptor)
    {
        return DefaultReader;
    }
}