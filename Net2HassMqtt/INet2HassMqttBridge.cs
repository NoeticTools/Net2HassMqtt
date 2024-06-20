namespace NoeticTools.Net2HassMqtt;

public interface INet2HassMqttBridge
{
    Task StartAsync();
    Task StopAsync();
}