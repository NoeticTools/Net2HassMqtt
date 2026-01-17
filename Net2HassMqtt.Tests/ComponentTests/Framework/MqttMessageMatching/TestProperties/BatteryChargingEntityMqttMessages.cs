namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.TestProperties;

public class BatteryChargingEntityMqttMessages(string clientId, string deviceId, string deviceFriendlyName) 
    : BinarySensorEntityMessagesBase(clientId, deviceId, deviceFriendlyName, 
                                     "batt1_charging", 
                                     "Battery Charging Status", 
                                     "battery_charging")
{
}