using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

public sealed class EntityMqttMessagesTestData<T, T2>(
    string entityFriendlyName,
    string nodeId,
    T unitOfMeasurement,
    string modelPropertyName,
    Action loopAction,
    Func<int, T2> getExpectedValue)
    where T : UnitOfMeasurement
{
    public string EntityFriendlyName { get; } = entityFriendlyName;

    public Func<int, T2> GetExpectedValue { get; } = getExpectedValue;

    public Action LoopAction { get; } = loopAction;

    public string NodeId { get; } = nodeId;

    public string ModelPropertyName { get; } = modelPropertyName;

    public T UnitOfMeasurement { get; } = unitOfMeasurement;
}