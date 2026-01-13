namespace Net2HassMqtt.Tests.ComponentTests.Framework.Messages;

public class DeviceMessageMatchers(string deviceFriendlyName, string deviceId)
{
    public BatteryChargingEntityMessages BatteryChargingEntity { get; } = new("net2hassmqtt_test_start", deviceFriendlyName, deviceId);

    public BridgeStateMessages BridgeState { get; } = new();
}