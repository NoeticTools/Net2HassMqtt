using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Framework;


namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads;

internal sealed class MqttCommandPayloadReader
{
    private readonly string _hassUoM;
    private readonly ILogger _logger;
    private readonly string _payload;

    public MqttCommandPayloadReader(string payload, string hassUoM, ILogger logger)
    {
        _payload = payload;
        _hassUoM = hassUoM;
        _logger = logger;
    }

    public object? ConvertTo(Type type)
    {
        var transforms = new Dictionary<Type, Func<object?>>
        {
            { typeof(bool), ToBool },
            { typeof(int), ToInt },
            { typeof(string), ToString },
            { typeof(TimeSpan), ToTimeSpan },
            { typeof(double), ToDouble }
        };

        if (transforms.TryGetValue(type, out var transform))
        {
            return transform();
        }

        _logger.LogWarning(LoggingEvents.MqttReceived, "No built in type conversion for received command method value type: {0}.", type.Name);

        return _payload;
    }

    public override string ToString()
    {
        return _payload;
    }

    private object? ToBool()
    {
        var validValues = new Dictionary<string, bool>
        {
            { "on", true },
            { "off", false },
            { "true", true },
            { "false", false },
            { "close", false },
            { "open", false },
            { "1", true },
            { "0", false }
        };

        if (validValues.TryGetValue(_payload.ToLower(), out var boolPayload))
        {
            return boolPayload;
        }

        return null;
    }

    private object? ToDouble()
    {
        try
        {
            return double.Parse(_payload);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(LoggingEvents.MqttReceived, exception, "Unable to transform MQTT payload to a double value.");
            return null;
        }
    }

    private object? ToInt()
    {
        try
        {
            return int.Parse(_payload);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(LoggingEvents.MqttReceived, exception, "Unable to transform MQTT payload to an integer value.");
            return null;
        }
    }

    private object? ToTimeSpan()
    {
        try
        {
            var transformLookup = new Dictionary<string, Func<object?>>
            {
                { HassUoMs.Seconds, () => TimeSpan.FromSeconds(double.Parse(_payload)) },
                { HassUoMs.Minutes, () => TimeSpan.FromMinutes(double.Parse(_payload)) },
                { HassUoMs.Hours, () => TimeSpan.FromHours(double.Parse(_payload)) },
                { HassUoMs.Days, () => TimeSpan.FromHours(double.Parse(_payload)) }
            };

            if (transformLookup.TryGetValue(_hassUoM, out var transform))
            {
                return transform();
            }

            if (TimeSpan.TryParse(_payload, out var result))
            {
                return result;
            }

            // this needs to be done by the configuration - hours, minutes, seconds?
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            _logger.LogWarning(LoggingEvents.MqttReceived, exception, "Unable to transform MQTT payload to a TimeSpan value.");
            return null;
        }
    }
}