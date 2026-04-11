using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Framework;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.TestProperties;

public sealed class TimestampEntityMqttMessages(string clientId, 
                                                string deviceId,
                                                string deviceFriendlyName,
                                                string nodeId,
                                                string nodeName,
                                                string domainName,
                                                string deviceClass)
    : MqttMessageMatcherBase(clientId, deviceId, deviceFriendlyName, 
                             nodeId, nodeName, domainName, deviceClass)
{
    public override MessageMatcher Config =>
        GetConfigurationMessage();

    public IMessageMatcher SendsState(DateTime timestamp)
    {
        var timeOffset = timestamp.ToDateTimeOffset();
        return new MessageMatcher($"{StateTopic}",
                                  $$"""
                                    {
                                      "attributes": {},
                                      "state": "{{timeOffset:o}}"
                                    }
                                    """);
    }

    public IMessageMatcher SendsState(int timestamp)
    {
        return new MessageMatcher($"{StateTopic}",
                                  $$"""
                                    {
                                      "attributes": {},
                                      "state": "{{timestamp}}"
                                    }
                                    """);
    }

    public IMessageMatcher SendsState(ComponentTestModel.TestStates state)
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