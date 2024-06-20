// ReSharper disable InconsistentNaming

#pragma warning disable IDE1006
namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;

internal sealed class StateWithDataMqttJson
{
    public StateWithDataMqttJson(object state, Dictionary<string, string> data)
    {
        this.state = state;
        this.data = data;
    }

    public Dictionary<string, string> data { get; set; }

    public object state { get; set; }
}