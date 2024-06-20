using FluentDate;
using NoeticTools.Net2HassMqtt.Entities;
using NoeticTools.Net2HassMqtt.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;


namespace NoeticTools.Net2HassMqtt;

/// <summary>
///     The bridge between the application and MQTT.
/// </summary>
internal sealed class Net2MqttBridge : INet2HassMqttBridge
{
    private readonly IEnumerable<Device> _devices;
    private readonly INet2HassMqttClient _mqttClient;

    internal Net2MqttBridge(INet2HassMqttClient mqttClient, IEnumerable<Device> devices)
    {
        _mqttClient = mqttClient;
        _devices = devices;
        _mqttClient.OnConnectedAsync += OnMqttConnectedAsync;
        _mqttClient.OnDisconnectedAsync += OnMqttDisconnectedAsync;
    }

    public async Task StartAsync()
    {
        await _mqttClient.StartAsync();
        await _mqttClient.WaitForConnection(3.Seconds());
        await _devices.ForeachAsync(device => device.StartAsync());
    }

    public async Task StopAsync()
    {
        await _devices.ForeachAsync(device => device.StopAsync());
        await _mqttClient.StopAsync();
    }

    public void Start()
    {
        StartAsync().GetAwaiter().GetResult();
    }

    public void Stop()
    {
        StopAsync().GetAwaiter().GetResult();
    }

    private async Task OnMqttConnectedAsync(EventArgs args)
    {
        await _devices.ForeachAsync(device => device.PublishConfigAsync());
        await Task.Delay(MqttConstants.DelayAfterPublishingConfig);
        await _devices.ForeachAsync(device => device.PublishStateAsync());
    }

    private Task OnMqttDisconnectedAsync(EventArgs args)
    {
        // todo
        return Task.CompletedTask;
    }
}