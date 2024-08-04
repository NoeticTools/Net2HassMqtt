// ReSharper disable InconsistentNaming

#pragma warning disable IDE1006
namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;

internal sealed class StateWithAttributeDataMqttJson
{
    public StateWithAttributeDataMqttJson(object state, Dictionary<string, string> attributes)
    {
        this.state = state;
        this.data = attributes;
    }

    public Dictionary<string, string> data { get; set; }

    public object state { get; set; }
}