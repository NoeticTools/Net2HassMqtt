namespace NoeticTools.Net2HassMqtt.Mqtt;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class HassMqttClientConnectionEventHandlers
{
    public delegate Task OnConnectionChangedHandlerAsync(EventArgs args);
}