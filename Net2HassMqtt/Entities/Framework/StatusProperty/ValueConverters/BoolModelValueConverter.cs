using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

internal class BoolModelValueConverter(ILogger logger) : ModelValueConverterBase(logger), IModelValueConverter
{
    private static readonly Dictionary<string, Func<object?, string>> BooleanReadersByHassDomains = new()
    {
        { HassDomains.BinarySensor.HassDomainName, ToOnOffValue },
        { HassDomains.Switch.HassDomainName, ToOnOffValue },
        { HassDomains.Valve.HassDomainName, ToOpenClosedValue },
        { HassDomains.Cover.HassDomainName, ToOpenClosedCover }
    };

    public bool CanConvert(ModelValueDescriptor valueDescriptor)
    {
        return BooleanReadersByHassDomains.ContainsKey(valueDescriptor.HassDomainName);
    }

    public Func<object?, string> GetConverter(ModelValueDescriptor valueDescriptor)
    {
        if (valueDescriptor.ModelPropertyType != typeof(bool))
        {
            ThrowConfigError($"A {valueDescriptor.HassDomainName} entity requires a boolean property. {valueDescriptor.ModelPropertyName}'s type is {valueDescriptor.ModelPropertyType.Name}.");
        }

        BooleanReadersByHassDomains.TryGetValue(valueDescriptor.HassDomainName, out var reader);
        return value => reader!(value);
    }

    private static string ToOnOffValue(object? value)
    {
        return (bool)value! ? MqttConstants.EntityOnState : MqttConstants.EntityOffState;
    }

    private static string ToOpenClosedCover(object? cover)
    {
        return (bool)cover! ? MqttConstants.EntityOpenState : MqttConstants.EntityClosedState;
    }

    private static string ToOpenClosedValue(object? value)
    {
        return (bool)value! ? MqttConstants.EntityOpenState : MqttConstants.EntityClosedState;
    }
}