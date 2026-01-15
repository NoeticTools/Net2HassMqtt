namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.TestProperties;

public class TestSensorsMessageMqttMatchers(string clientId, string deviceFriendlyName, string deviceId)
{
    public BatteryChargingEntityMqttMessages BatteryChargingEntity { get; } = new(clientId, deviceId, deviceFriendlyName);

    public DoorIsOpenEntityMqttMessages DoorIsOpenEntity { get; } = new(clientId, deviceId, deviceFriendlyName);

    public CurrentStateEntityMqttMessages CurrentStateEntity { get; } = new(clientId, deviceId, deviceFriendlyName);

    public BridgeStateMessages BridgeState { get; } = new();

    public IMessageMatcher Any(int maximumNumberOfMessages = int.MaxValue)
    {
        return new AnyMessagesMatcher(maximumNumberOfMessages);
    }
}