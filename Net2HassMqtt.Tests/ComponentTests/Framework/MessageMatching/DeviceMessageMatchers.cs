namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public class DeviceMessageMatchers(string clientId, string deviceFriendlyName, string deviceId)
{
    public BatteryChargingEntityMessages BatteryChargingEntity { get; } = new("net2hassmqtt_test_start", deviceId, deviceFriendlyName);

    public DoorIsOpenEntityMessages DoorIsOpenEntity { get; } = new("net2hassmqtt_test_start", deviceId, deviceFriendlyName);

    public BridgeStateMessages BridgeState { get; } = new();

    public IMessageMatcher Any()
    {
        return Any(1);
    }

    public IMessageMatcher Any(int maximumNumberOfMessages)
    {
        return new AnyMessagesMatcher(maximumNumberOfMessages);
    }
}