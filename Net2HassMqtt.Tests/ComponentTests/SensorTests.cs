using FluentDate;
using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class SensorTests : ComponentTestsBase
{
    [SetUp]
    public void Setup()
    {
        BaseSetup();
        Client.Setup.ConnectsImmediately();
    }

    [TearDown]
    public void TearDown()
    {
        BaseTearDown();
    }

    [Test]
    public async Task EnumSensorTest()
    {
        DeviceBuilder.SetupCurrentStateEnumSensor(Model);

        var result = await Run(BumpCurrentState, 4);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMessages.Verify.SequenceWas(
        [
            MessageMatchers.BridgeState.Online,
            MessageMatchers.CurrentStateEntity.Config,

            MessageMatchers.CurrentStateEntity.Is(ComponentTestModel.TestStates.StateTwo),
            MessageMatchers.CurrentStateEntity.Is(ComponentTestModel.TestStates.StateThree),
            MessageMatchers.CurrentStateEntity.Is(ComponentTestModel.TestStates.StateOne),
            MessageMatchers.CurrentStateEntity.Is(ComponentTestModel.TestStates.StateTwo),

            MessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    private void BumpCurrentState()
    {
        Model.CurrentState = Model.CurrentState switch
        {
            ComponentTestModel.TestStates.StateOne => ComponentTestModel.TestStates.StateTwo,
            ComponentTestModel.TestStates.StateTwo => ComponentTestModel.TestStates.StateThree,
            ComponentTestModel.TestStates.StateThree => ComponentTestModel.TestStates.StateOne,
            _ => Model.CurrentState
        };
    }
}