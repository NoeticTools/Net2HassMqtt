using CommunityToolkit.Mvvm.ComponentModel;
using NoeticTools.Net2HassMqtt.Entities.Framework;


namespace NoeticTools.Net2HassMqtt.QuickStartDemoApp.SampleEntityModels;

public partial class QuickStartDemoModel : ObservableObject
{
    [ObservableProperty] private bool _batteryCharging;

    /// <summary>
    /// Test property that will be automatically read on every TestEvent event and added as an attribute.
    /// </summary>
    public string ModelName => "Quick start demo model";

    public event EventHandler<HassEventArgs>? Event1;

    public event EventHandler<HassEventArgs>? Event2;

    public void OnKeyPressed(char keyChar)
    {
        HandleEvent1KeyPresses(keyChar);
        HandleEvent2KeyPresses(keyChar);
    }

    private void HandleEvent1KeyPresses(char keyChar)
    {
        var messages = new Dictionary<char, string>()
        {
            {'a', "PressedA"},
            {'b', "PressedB"},
        };
        if (!messages.TryGetValue(keyChar, out var eventType))
        {
            return;
        }

        Console.WriteLine($"\nFiring event 1 '{eventType}'.\n");

        var attributes = new Dictionary<string, string>()
        {
            {"From event attribute 1", "An example attribute value defined in the client model."},
            {"From event attribute 2", "Another attribute value set when the event is fired in app code."},
        };
        Event1?.Invoke(this, new HassEventArgs(eventType, attributes));
    }

    private void HandleEvent2KeyPresses(char keyChar)
    {
        var messages = new Dictionary<char, string>()
        {
            {'c', "PressedC"},
            {'d', "PressedD"},
        };
        if (!messages.TryGetValue(keyChar, out var eventType))
        {
            return;
        }

        Console.WriteLine($"\nFiring event 2 '{eventType}'.\n");

        Event2?.Invoke(this, new HassEventArgs(eventType));
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