namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;
// ReSharper disable InconsistentNaming

internal class EventWithAttributeDataMqttJson
{
    public EventWithAttributeDataMqttJson(Dictionary<string, string> eventData, Dictionary<string, string> attributes)
    {
        state = eventData;
        data = attributes;
    }

    public Dictionary<string, string> data { get; set; }

    public object state { get; set; }
}