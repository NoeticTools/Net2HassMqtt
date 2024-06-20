using System.Text.Json.Serialization;


namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;

internal sealed class AvailabilityStateMqttJson
{
    public AvailabilityStateMqttJson(string state)
    {
        State = state;
    }

    [JsonPropertyName("state")]
    public string State { get; set; }
}