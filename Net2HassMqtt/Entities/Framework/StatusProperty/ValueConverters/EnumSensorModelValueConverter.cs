using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

internal sealed class EnumSensorModelValueConverter(ILogger logger) : ModelValueConverterBase(logger), IModelValueConverter
{
    public bool CanConvert(ModelValueDescriptor valueDescriptor)
    {
        return valueDescriptor.HassDomainName == HassDomains.Sensor.HassDomainName &&
               valueDescriptor.HassDeviceClass == SensorDeviceClass.Enum.HassDeviceClassName;
    }

    public Func<object?, string> GetConverter(ModelValueDescriptor valueDescriptor)
    {
        if (!valueDescriptor.ModelPropertyType.IsEnum)
        {
            ThrowConfigError($"An enum entity requires an enum property. {valueDescriptor.ModelPropertyName}'s type is {valueDescriptor.ModelPropertyName}.");
        }

        return DefaultReader;
    }
}