using CommunityToolkit.Mvvm.ComponentModel;
using NoeticTools.Net2HassMqtt.Entities.Framework;


namespace NoeticTools.Net2HassMqtt.QuickStartDemoApp.SampleEntityModels;

public class QuickStartDemoModel : ObservableObject
{
    private bool _batteryCharging;

    public bool BatteryCharging
    {
        get => _batteryCharging;
        set
        {
            if (SetProperty(ref _batteryCharging, value))
            {
                Console.WriteLine($" Battery is charging: {BatteryCharging}");
            }
        }
    }

    public HaEvent TestEvent = new (new [] {"A", "B"});
}

// A Better way of doing the same thing as above ...
public partial class MyDemoModel_Alt : ObservableObject
{
    [ObservableProperty]
    private bool _lightSwitch;

    private void Example()
    {
        // The property is auto generated. Just to show it exists:
        LightSwitch = true;
    }
}