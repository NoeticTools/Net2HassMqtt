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

    public IMessageMatcher SendsState<T>(T value)
    {
        if (value is DateTimeOffset timestamp)
        {
            return SendsState(timestamp);
        }
        if (value is DateOnly dateOnly)
        {
            return SendsState(dateOnly);
        }
        if (value is DateTime dateTime)
        {
            return SendsState(dateTime);
        }
        if (value is TimeSpan timespan)
        {
            return SendsState(timespan);
        }
        if (value is int intValue)
        {
            return SendsState(intValue);
        }
        if (value is double doubleValue)
        {
            return SendsState(doubleValue);
        }
        if (value is ComponentTestModel.TestStates state)
        {
            return SendsState(state);
        }
        throw new NotSupportedException($"Type {typeof(T)} is not supported for state matching.");
    }

    public IMessageMatcher SendsState(DateTimeOffset timestamp)
    {
        var timeString = timestamp.ToIso8601String();
        return new MessageMatcher($"{StateTopic}",
                                  $$"""
                                    {
                                      "attributes": {},
                                      "state": "{{timeString}}"
                                    }
                                    """);
    }

    public IMessageMatcher SendsState(DateOnly timestamp)
    {
        var timeString = timestamp.ToIso8601String();
        return new MessageMatcher($"{StateTopic}",
                                  $$"""
                                    {
                                      "attributes": {},
                                      "state": "{{timeString}}"
                                    }
                                    """);
    }

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

    public IMessageMatcher SendsState(double timespan)
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