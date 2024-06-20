using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt;


namespace NoeticTools.Net2HassMqtt.Entities;

internal sealed class DeviceFactory
{
    private readonly INet2HassMqttClient _client;
    private readonly ILoggerFactory _loggerFactory;

    public DeviceFactory(INet2HassMqttClient client, ILoggerFactory loggerFactory)
    {
        _client = client;
        _loggerFactory = loggerFactory;
    }

    public Device Create(DeviceConfig config)
    {
        return new Device(config, _client, _loggerFactory.CreateLogger<Device>());
    }
}