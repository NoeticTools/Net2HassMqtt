using System.Text.Json;
using System.Text.Json.Serialization;
using FluentDate;


namespace NoeticTools.Net2HassMqtt.Mqtt;

public static class MqttConstants
{
    public static string DefaultHassMqttClientId { get; } = "homeassistant";

    /// <summary>
    ///     A delay after publishing Home Assistant MQTT discovery information before starting to send MQTT status messages.
    /// </summary>
    public static TimeSpan DelayAfterPublishingConfig { get; } = 100.Milliseconds();

    /// <summary>
    ///     A delay after publishing MQTT last will before closing the client. Used during shutdown.
    /// </summary>
    public static TimeSpan DelayAfterPublishingLastWill { get; } = 100.Milliseconds();

    public static string EntityOffState { get; } = "OFF";

    public static string EntityOnState { get; } = "ON";

    public static string EntityOffPayload { get; } = "OFF";

    public static string EntityOnPayload { get; } = "ON";

    public static JsonSerializerOptions MqttJsonSerialiseOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string OffLineState { get; } = "offline";

    public static string OnLineState { get; } = "online";

    public static string WillSubTopic { get; } = "bridge/state";

    public static string EntityOpenState { get; } = "open";

    public static string EntityClosedState { get; } = "closed";
}