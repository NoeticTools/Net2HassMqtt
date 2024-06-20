namespace NoeticTools.Net2HassMqtt.Mqtt;

internal interface IMqttSubscriber
{
    /// <summary>
    ///     A message from an MQTT topic subscription has been received.
    /// </summary>
    void OnReceived(ReceivedMqttMessage message);
}