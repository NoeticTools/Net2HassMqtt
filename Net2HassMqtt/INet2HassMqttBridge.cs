namespace NoeticTools.Net2HassMqtt;

public interface INet2HassMqttBridge {
    Task<bool> StartAsync();
    Task StopAsync();
}