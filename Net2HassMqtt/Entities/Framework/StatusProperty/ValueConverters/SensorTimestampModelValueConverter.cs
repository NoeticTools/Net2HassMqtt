using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Framework;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

internal sealed class SensorTimestampModelValueConverter(ILogger logger) : ModelValueConverterBase(logger), IModelValueConverter
{
    public bool CanConvert(ModelValueDescriptor valueDescriptor)
    {
        return valueDescriptor.HassDomainName == HassDomains.Sensor.HassDomainName &&
               valueDescriptor.HassDeviceClass == SensorDeviceClass.Timestamp.HassDeviceClassName;
    }

    public Func<object?, string> GetConverter(ModelValueDescriptor valueDescriptor)
    {
        if (valueDescriptor.ModelPropertyType == typeof(int) || valueDescriptor.ModelPropertyType == typeof(double))
        {
            // todo - HASS requires an ISO 8601 string with time zone info. Drop this in next major revision.
            return DefaultReader;
        }

        if (valueDescriptor.ModelPropertyType == typeof(DateTime))
        {
            return value => DefaultReader(((DateTime)value!).ToIso8601String());
        }

        ThrowUnableToFindReaderException(valueDescriptor);
        return value => string.Empty; // never gets here
    }
}