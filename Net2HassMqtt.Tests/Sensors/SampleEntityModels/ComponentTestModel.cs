using CommunityToolkit.Mvvm.ComponentModel;


#pragma warning disable MVVMTK0042

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
    private DateOnly _dateOnlyDate = new(2026, 12, 29);

    [ObservableProperty]
    private DateTimeOffset _dateTimeOffsetTimestamp = new(2026, 12, 30, 9, 31, 42, TimeSpan.FromHours(1));

    [ObservableProperty]
    private DateTime _dateTimeTimestamp = new(2026, 12, 31, 9, 31, 42);

    [ObservableProperty]
    private bool _doorIsOpen;

    [ObservableProperty]
    private int _intDuration;

    [ObservableProperty]
    private double _doubleDuration;

    [ObservableProperty]
    private int _intTimestamp = 1234; // todo - should be unix timestamp? long?

    [ObservableProperty]
    private TimeSpan _timespanDuration = TimeSpan.Zero; // todo - should be unix timestamp? long?
}