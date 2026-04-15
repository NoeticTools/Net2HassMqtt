using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Framework;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

internal sealed class DateSensorModelValueConverter(ILogger logger) : ModelValueConverterBase(logger), IModelValueConverter
{
    public bool CanConvert(ModelValueDescriptor valueDescriptor)
    {
        return valueDescriptor.HassDomainName == HassDomains.Sensor.HassDomainName &&
               valueDescriptor.HassDeviceClass == SensorDeviceClass.Date.HassDeviceClassName;
    }

    public Func<object?, string> GetConverter(ModelValueDescriptor valueDescriptor)
    {
        if (valueDescriptor.ModelPropertyType != typeof(DateOnly))
        {
            ThrowConfigError($"A date sensor requires a DateOnly model property. Model property {valueDescriptor.ModelPropertyName} is of type {valueDescriptor.ModelPropertyType}.");
        }

        return value => DefaultReader(((DateOnly)value!).ToIso8601String());
    }
}