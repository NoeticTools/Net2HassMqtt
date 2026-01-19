namespace Net2HassMqtt.Tests.Entities.Framework;

public enum TestEventTypes1
{
    EventType1 = 3,
    EventType2 = 5
}

[TestFixture]
internal class HassEventArgsTests
{
    [TestCase(TestEventTypes1.EventType1, 3)]
    [TestCase(TestEventTypes1.EventType2, 5)]
    public void Tests(TestEventTypes1 eventType, int expectedId)
    {
        //var target = new HassEventArgs<TestEventTypes1>(eventType);

        //Assert.That(expectedId, Is.EqualTo(target.EventType));
    }
}