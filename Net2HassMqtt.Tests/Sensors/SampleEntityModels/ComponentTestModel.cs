using CommunityToolkit.Mvvm.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

    [ObservableProperty]
    private DateTime _dateTimeTimestamp = new DateTime(2026, 12, 31, 9, 31, 42);

    [ObservableProperty]
    private int _intTimestamp = 1234; // todo - should be unix timestamp? long?
}