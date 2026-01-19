using CommunityToolkit.Mvvm.ComponentModel;


namespace Net2HassMqtt.Tests.Sensors.SampleEntityModels;

public partial class ComponentTestModel : ObservableObject
{
    public enum TestStates
    {
        StateOne,
        StateTwo,
        StateThree
    }

    [ObservableProperty]
    private bool _batteryCharging;

    [ObservableProperty]
    private TestStates _currentState;

    [ObservableProperty]
    private bool _doorIsOpen;
}