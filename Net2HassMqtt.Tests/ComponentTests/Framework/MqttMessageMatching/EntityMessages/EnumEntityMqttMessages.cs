using Net2HassMqtt.Tests.Sensors.SampleEntityModels;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.EntityMessages;

public sealed class EnumEntityMqttMessages(string clientId, string deviceId, string deviceFriendlyName)
    : MqttMessageMatcherBase(clientId, deviceId, deviceFriendlyName,
                             "current_state",
                             "Current State",
                             "sensor",
                             "enum", null)
{
    public override MessageMatcher Config =>
        GetConfigurationMessage("""
                                              [
                                                  "StateOne",
                                                  "StateTwo",
                                                  "StateThree"
                                                ]
                                              """);

    public IMessageMatcher Is(ComponentTestModel.TestStates state)
    {
        return new MessageMatcher($"{StateTopic}",
                                  $$"""
                                    {
                                      "attributes": {},
                                      "state": "{{state}}"
                                    }
                                    """);
    }
}