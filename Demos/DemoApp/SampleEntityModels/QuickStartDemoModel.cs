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
        var properties = new Dictionary<string, string>()
        {
            {"From event attribute 1", "An example attribute value defined in the client model."},
            {"From event attribute 2", "Another attribute value set when the event is fired in app code."},
        };
        TestEvent?.Invoke(this, new HassEventArgs(eventType, properties));
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