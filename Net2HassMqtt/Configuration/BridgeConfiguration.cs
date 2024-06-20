using MQTTnet.Extensions.ManagedClient;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using NoeticTools.Net2HassMqtt.Exceptions;


namespace NoeticTools.Net2HassMqtt.Configuration;

public sealed class BridgeConfiguration
{
    internal List<object> Devices { get; set; } = [];

    internal ManagedMqttClientOptions? MqttOptions { get; private set; }

    public BridgeConfiguration WithMqttOptions(ManagedMqttClientOptions mqttOptions)
    {
        MqttOptions = mqttOptions;
        return this;
    }

    public INet2HassMqttBridge Build()
    {
        return HassMqttBridgeBuilder.Build(this);
    }

    public BridgeConfiguration HasDevice(DeviceBuilder deviceBuilder)
    {
        Devices.Add(deviceBuilder.DeviceConfig);
        return this;
    }

    public void Validate()
    {
        if (MqttOptions == null)
        {
            throw new Net2HassMqttConfigurationException("MQTT options are required.");
        }

        var devices = Devices.Cast<DeviceConfig>().ToList();
        foreach (var device in devices)
        {
            var errors = device.Validate();
            if (errors.Any())
            {
                throw new Net2HassMqttConfigurationException($"Unable to build configuration: {string.Join(", ", errors)}");
            }

            var devicesWithSameId = devices.Where(x => x.DeviceId == device.DeviceId).ToList();
            if (devicesWithSameId.Count <= 1)
            {
                continue;
            }

            var message = $"Each device requires a unique ID. {devicesWithSameId.Count} devices have the ID '{device.DeviceId}'";
            throw new Net2HassMqttConfigurationException(message);
        }

        if (MqttOptions == null)
        {
            throw new Net2HassMqttConfigurationException("MQTT options are required.");
        }
    }
}