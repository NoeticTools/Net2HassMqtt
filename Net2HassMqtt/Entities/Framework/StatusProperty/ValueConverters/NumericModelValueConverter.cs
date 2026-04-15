using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

/// <summary>
/// Provides a HASS MQTT value converter for numeric status properties (int and double) for supported HASS domains (humidifier, sensor, number).
/// </summary>
/// <param name="logger">The logger instance.</param>
internal sealed class NumericModelValueConverter(ILogger logger) : ModelValueConverterBase(logger), IModelValueConverter
{
    private static readonly HashSet<string> ValueHassDomains =
    [
        HassDomains.Humidifier.HassDomainName,
        HassDomains.Sensor.HassDomainName,
        HassDomains.Number.HassDomainName
    ];

    /// <summary>
    /// Returns true if the provided ModelValueDescriptor is compatible with this converter,
    /// meaning it belongs to one of the supported HASS domains and has a model property type of either int or double.
    /// </summary>
    /// <param name="valueDescriptor">The descriptor of the model value to check.</param>
    /// <returns>True if the converter can handle the specified model value; otherwise, false.</returns>
    public bool CanConvert(ModelValueDescriptor valueDescriptor)
    {
        return ValueHassDomains.Contains(valueDescriptor.HassDomainName) &&
               (
                   valueDescriptor.ModelPropertyType == typeof(int) ||
                   valueDescriptor.ModelPropertyType == typeof(double)
               );
    }

    /// <summary>
    /// The converter function that converts the model property value to a string representation suitable for MQTT payloads, specifically for numeric values (int and double).
    /// It uses the default reader implementation provided by the base class, which handles the conversion logic based on the property type.
    /// </summary>
    /// <param name="valueDescriptor">The descriptor of the model value to convert.</param>
    /// <returns>A function that converts the model value to a string representation suitable for MQTT payloads.</returns>
    public Func<object?, string> GetConverter(ModelValueDescriptor valueDescriptor)
    {
        return DefaultReader;
    }
}