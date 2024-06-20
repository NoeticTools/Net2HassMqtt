// ReSharper disable UnusedMember.Global

using System.Text.Json.Serialization;


namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;

public class AvailabilityMqttJson
{
    [JsonPropertyName("payload_available")]
    public string PayloadAvailable { get; set; } = MqttConstants.OnLineState;

    [JsonPropertyName("payload_not_available")]
    public string PayloadNotAvailable { get; set; } = MqttConstants.OffLineState;

    [JsonPropertyName("topic")]
    public string Topic { get; set; } = "net2hassmqtt/bridge/status";

    [JsonPropertyName("value_template")]
    public string ValueTemplate { get; set; } = "{{ value_json.state }}";
}