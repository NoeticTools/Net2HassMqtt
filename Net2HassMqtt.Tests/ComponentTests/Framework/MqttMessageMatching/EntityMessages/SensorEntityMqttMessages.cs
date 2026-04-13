using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Framework;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.EntityMessages;

public sealed class SensorEntityMqttMessages(
    string clientId,
    string deviceId,
    string deviceFriendlyName,
    string nodeId,
    string nodeName,
    string deviceClass,
    string unitOfMeasurementOrNone)
    : MqttMessageMatcherBase(clientId, deviceId, deviceFriendlyName, 
                             nodeId, nodeName, HassDomains.Sensor.HassDomainName, 
                             deviceClass, unitOfMeasurementOrNone)
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

    public IMessageMatcher SendsState(TimeSpan timespan)
    {
        return new MessageMatcher($"{StateTopic}",
                                  $$"""
                                    {
                                      "attributes": {},
                                      "state": "{{timespan:o}}"
                                    }
                                    """);
    }

    public IMessageMatcher SendsState(int timespan)
    {
        return new MessageMatcher($"{StateTopic}",
                                  $$"""
                                    {
                                      "attributes": {},
                                      "state": "{{timespan}}"
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