namespace Net2HassMqtt.Tests.ComponentTests.Framework.Messages;

public class BatteryChargingEntityMessages(string clientId, string deviceFriendlyName, string deviceId) 
    : BinarySensorMessagesBase(clientId, deviceFriendlyName, deviceId, "batt1_charging", "battery_charging")
{
}