namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching.TestProperties;

public sealed class DoorIsOpenEntityMessages(string clientId, string deviceId, string deviceFriendlyName) 
    : BinarySensorEntityMessagesBase(clientId, deviceId, deviceFriendlyName, 
                                     "door_is_open", 
                                     "Door Open Status", 
                                     "door")
{
}