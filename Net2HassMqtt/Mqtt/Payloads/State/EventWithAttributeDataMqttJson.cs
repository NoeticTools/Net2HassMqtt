using System.Text.Json.Serialization;


namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;

internal class EventWithAttributeDataMqttJson
{
    public EventWithAttributeDataMqttJson(string eventType, Dictionary<string, string> attributes)
    {
        Event = new Dictionary<string, string>() { { "event_type", eventType } };
        Attributes = attributes;
    }

    [JsonPropertyName("attributes")]
    public Dictionary<string, string> Attributes { get; set; }

    [JsonPropertyName("event")]
    public Dictionary<string, string> Event { get; set; }
}