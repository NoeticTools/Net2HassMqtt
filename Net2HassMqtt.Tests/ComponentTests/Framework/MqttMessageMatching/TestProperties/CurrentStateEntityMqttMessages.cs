using Net2HassMqtt.Tests.Sensors.SampleEntityModels;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.TestProperties;

public sealed class CurrentStateEntityMqttMessages(string clientId, string deviceId, string deviceFriendlyName) 
    : MqttMessageMatcherBase(clientId, deviceId, deviceFriendlyName, 
                               "current_state", 
                               "Current State", 
                               "sensor", 
                               "enum")
{
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
    
    public override MessageMatcher Config =>
        GetConfigurationMessageWithoutOptions("""
                                              [
                                                  "StateOne",
                                                  "StateTwo",
                                                  "StateThree"
                                                ]
                                              """);

}