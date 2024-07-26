using NoeticTools.Net2HassMqtt.Configuration;


namespace NoeticTools.Net2HassMqtt.Mqtt;

internal interface IMqttEntity
{
    Task StartAsync();
    Task StopAsync();
    Task PublishConfigAsync(DeviceConfig deviceConfig);
}