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

    public async Task<bool> StartAsync()
    {
        await _mqttClient.StartAsync();
        if (false == await _mqttClient.WaitForConnection(3.Seconds()))
            return false;
        foreach (var device in _devices)
        {
            await device.StartAsync();
        }
        return true;
    }

    public async Task StopAsync()
    {
        await _devices.ForeachAsync(device => device.StopAsync());
        await _mqttClient.StopAsync();
    }

    public bool Start()
    {
        return StartAsync().GetAwaiter().GetResult();
    }

    public void Stop()
    {
        StopAsync().GetAwaiter().GetResult();
    }

    private async Task OnMqttConnectedAsync(EventArgs args)
    {
        Console.WriteLine("--- 98 ---");

        await _devices.ForeachAsync(device => device.PublishConfigAsync());
        await Task.Delay(MqttConstants.DelayAfterPublishingConfig);
        await _devices.ForeachAsync(device => device.PublishStateAsync());
    }

    private Task OnMqttDisconnectedAsync(EventArgs args)
    {
        return Task.CompletedTask;
    }
}