using System.Text.Json.Serialization;
using NoeticTools.Net2HassMqtt.Configuration;


namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads;

public sealed class DeviceMqttJson
{
    public DeviceMqttJson(DeviceConfig config)
    {
        Identifiers = config.Identifiers;
        Manufacturer = config.Manufacturer;
        Model = config.Model;
        Name = config.Name;
    }

    [JsonPropertyName("identifiers")]
    public string[] Identifiers { get; set; }

    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}