
#pragma warning disable IDE1006
using System.Text.Json.Serialization;

namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;

internal sealed class StateWithAttributeDataMqttJson
{
    public StateWithAttributeDataMqttJson(object state, Dictionary<string, string> attributes)
    {
        State = state;
        Attributes = attributes;
    }

    [JsonPropertyName("attributes")]
    public Dictionary<string, string> Attributes { get; set; }

    [JsonPropertyName("state")]
    public object State { get; set; }
}