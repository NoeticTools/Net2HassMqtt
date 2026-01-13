namespace Net2HassMqtt.Tests.ComponentTests.Framework.Messages;

public class BatteryChargingEntityMessages(string clientId, string deviceId, string deviceFriendlyName) 
    : BinarySensorMessagesBase(clientId, deviceId, deviceFriendlyName, 
                               "batt1_charging", 
                               "Battery Charging Status", 
                               "battery_charging")
{
}