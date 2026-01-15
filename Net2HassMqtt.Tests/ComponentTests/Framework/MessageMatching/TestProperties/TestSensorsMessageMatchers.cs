namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching.TestProperties;

public class TestSensorsMessageMatchers(string clientId, string deviceFriendlyName, string deviceId)
{
    public BatteryChargingEntityMessages BatteryChargingEntity { get; } = new(clientId, deviceId, deviceFriendlyName);

    public DoorIsOpenEntityMessages DoorIsOpenEntity { get; } = new(clientId, deviceId, deviceFriendlyName);

    public CurrentStateEntityMessages CurrentStateEntity { get; } = new(clientId, deviceId, deviceFriendlyName);

    public BridgeStateMessages BridgeState { get; } = new();

    public IMessageMatcher Any(int maximumNumberOfMessages = int.MaxValue)
    {
        return new AnyMessagesMatcher(maximumNumberOfMessages);
    }
}