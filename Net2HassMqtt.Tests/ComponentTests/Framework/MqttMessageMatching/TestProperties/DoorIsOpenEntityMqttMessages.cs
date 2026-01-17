namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.TestProperties;

public sealed class DoorIsOpenEntityMqttMessages(string clientId, string deviceId, string deviceFriendlyName) 
    : BinarySensorEntityMessagesBase(clientId, deviceId, deviceFriendlyName, 
                                     "door_is_open", 
                                     "Door Open Status", 
                                     "door")
{
}