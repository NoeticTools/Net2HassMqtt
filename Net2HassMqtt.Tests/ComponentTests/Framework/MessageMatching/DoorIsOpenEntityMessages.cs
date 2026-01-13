namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public sealed class DoorIsOpenEntityMessages(string clientId, string deviceId, string deviceFriendlyName) 
    : BinarySensorMessagesBase(clientId, deviceId, deviceFriendlyName, 
                               "door_is_open", 
                               "Door Open Status", 
                               "door")
{
}