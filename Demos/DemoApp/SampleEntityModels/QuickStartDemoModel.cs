using CommunityToolkit.Mvvm.ComponentModel;
using NoeticTools.Net2HassMqtt.Entities.Framework;


namespace NoeticTools.Net2HassMqtt.QuickStartDemoApp.SampleEntityModels;

public partial class QuickStartDemoModel : ObservableObject
{
    [ObservableProperty] private bool _batteryCharging;

    public string ModelName => "Quick start demo model";

    public event EventHandler<HassEventArgs>? TestEvent;

    /// <summary>
    /// For testing only. Normally the event would be fired within its owning class based on some domain state/events.
    /// </summary>
    public void FireEvent(string eventType)
    {
        TestEvent?.Invoke(this, new HassEventArgs(eventType));
    }
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