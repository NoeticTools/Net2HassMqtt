namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public abstract class BinarySensorEntityMessagesBase(
    string clientId,
    string deviceId,
    string deviceName,
    string nodeId,
    string nodeName,
    string deviceClass)
    : MessagesBase(clientId, deviceId, deviceName, nodeId, nodeName, "binary_sensor", deviceClass)
{
    public IMessageMatcher Off =>
        new MessageMatcher($"{StateTopic}",
                           """
                           {
                             "attributes": {},
                             "state": "OFF"
                           }
                           """);

    public IMessageMatcher On =>
        new MessageMatcher($"{StateTopic}",
                           """
                           {
                             "attributes": {},
                             "state": "ON"
                           }
                           """);
}