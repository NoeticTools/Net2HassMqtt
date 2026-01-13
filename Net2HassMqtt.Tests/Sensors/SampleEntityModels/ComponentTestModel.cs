using CommunityToolkit.Mvvm.ComponentModel;
using NoeticTools.Net2HassMqtt.Entities.Framework;


namespace Net2HassMqtt.Tests.Sensors.SampleEntityModels;

public partial class ComponentTestModel : ObservableObject
{
    [ObservableProperty] private bool _batteryCharging;
    [ObservableProperty] private TestStates _currentState;

    public enum TestStates
    {
        StateOne,
        StateTwo, 
        StateThree
    }
}
