using Microsoft.Extensions.Logging;


namespace NoeticTools.Net2HassMqtt.Framework;

internal static class LoggingEvents
{
    public static EventId EntityState { get; } = new(10, "Entity state publishing.");
    public static EventId MqttConnection { get; } = new(101, "MQTT connection to broker state.");
    public static EventId MqttReceived { get; } = new(100, "MQTT subscription received a message.");
}