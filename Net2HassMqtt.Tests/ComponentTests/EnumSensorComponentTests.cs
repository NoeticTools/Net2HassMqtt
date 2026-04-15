using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class EnumSensorComponentTests : ComponentTestsBase
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
        DeviceBuilder.HasEnumSensor(config => config.OnModel(Model)
                                                    .WithStatusProperty(nameof(ComponentTestModel.CurrentState))
                                                    .WithFriendlyName("Current State")
                                                    .WithNodeId("current_state"));

        var result = await Run(BumpCurrentState, 4);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMqttMessages.Verify.MatchSequence(
        [
            MqttMessageMatchers.BridgeState.Online,
            MqttMessageMatchers.EnumEntity.Config,

            MqttMessageMatchers.EnumEntity.Is(ComponentTestModel.TestStates.StateTwo),
            MqttMessageMatchers.EnumEntity.Is(ComponentTestModel.TestStates.StateThree),
            MqttMessageMatchers.EnumEntity.Is(ComponentTestModel.TestStates.StateOne),
            MqttMessageMatchers.EnumEntity.Is(ComponentTestModel.TestStates.StateTwo),

            MqttMessageMatchers.Any(9)
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