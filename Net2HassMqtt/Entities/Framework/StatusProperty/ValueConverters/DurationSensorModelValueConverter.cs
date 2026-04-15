using System.Globalization;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

/// <summary>
/// Converts TimeSpan model property values for duration sensors to a HASS MQTT value string using the the sensor's configured unit of measurement (seconds, minutes, hours, or days).
/// If the unit of measurement is missing or invalid, it defaults to converting the TimeSpan to total minutes.
/// This converter is specifically designed for HASS sensor entities with a device class of "duration" and a model property type of TimeSpan.
/// </summary>
/// <param name="logger">The logger instance.</param>
internal sealed class DurationSensorModelValueConverter(ILogger logger) : ModelValueConverterBase(logger), IModelValueConverter
{
    private static readonly Dictionary<string, Func<TimeSpan, double>> TimeSpanDurationReadersByUoM = new()
    {
        { HassUoMs.Seconds, span => span.TotalSeconds },
        { HassUoMs.Minutes, span => span.TotalMinutes },
        { HassUoMs.Hours, span => span.TotalHours },
        { HassUoMs.Days, span => span.TotalDays }
    };

    public bool CanConvert(ModelValueDescriptor valueDescriptor)
    {
        return valueDescriptor.HassDomainName == HassDomains.Sensor.HassDomainName &&
               valueDescriptor.HassDeviceClass == SensorDeviceClass.Duration.HassDeviceClassName &&
               valueDescriptor.ModelPropertyType == typeof(TimeSpan);
    }

    public Func<object?, string> GetConverter(ModelValueDescriptor valueDescriptor)
    {
        if (TimeSpanDurationReadersByUoM.TryGetValue(valueDescriptor.HassUoM!, out var timespanReader))
        {
            return value => DefaultReader(timespanReader((TimeSpan)value!));
        }

        // todo - throw exception if uom is missing or invalid. Drop this in next major revision.

        return value =>
        {
            // todo - this assumes that an int duration is always in minutes. Drop this in next major revision.
            var result = ((TimeSpan)value!).TotalMinutes.ToString(CultureInfo.CurrentCulture);
            return DefaultReader(result);
        };
    }
}