using FluentDate;
using Net2HassMqtt.Tests.ComponentTests.Framework;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class BinarySensorTests : ComponentTestsBase
{
    [SetUp]
    public void Setup()
    {
        base.BaseSetup();
        DeviceBuilder.SetupDoorIsOpenBinarySensor(Model);
        Client.Setup.ConnectsImmediately();
    }

    [TearDown]
    public void TearDown()
    {
        Task.Delay(10.Milliseconds()).Wait();   // todo: code smell, required to avoid intermittency in number of messages received
                                                // fix in separate issue
    }

    [Test]
    public async Task DoorTest()
    {
        var result = await Run(() => Model.DoorIsOpen = !Model.DoorIsOpen, 5);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMessages.Verify.ValidateSequenceWas(
        [
            MessageMatchers.BridgeState.Online,
            MessageMatchers.DoorIsOpenEntity.Config,

            MessageMatchers.DoorIsOpenEntity.On,
            MessageMatchers.DoorIsOpenEntity.Off,
            MessageMatchers.DoorIsOpenEntity.On,

            MessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }
}