// ReSharper disable InconsistentNaming

#pragma warning disable IDE1006
namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;

internal sealed class ActionWithDataMqttJson
{
    public ActionWithDataMqttJson(string action, Dictionary<string, string>? data = null)
    {
        this.action = action;
        this.data = data ?? new Dictionary<string, string>();
    }

    public Dictionary<string, string> data { get; set; }

    public string action { get; set; }
}