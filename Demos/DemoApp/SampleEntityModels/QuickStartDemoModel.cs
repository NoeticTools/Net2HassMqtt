using CommunityToolkit.Mvvm.ComponentModel;
using NoeticTools.Net2HassMqtt.Entities.Framework;


namespace NoeticTools.Net2HassMqtt.QuickStartDemoApp.SampleEntityModels;

public partial class QuickStartDemoModel : ObservableObject
{
    [ObservableProperty] private bool _batteryCharging;

    public enum EventTypes
    {
        KeyPressE = 0,
        KeyPressF = 1
    }

    /// <summary>
    /// Test property that will be automatically read on every TestEvent event and added as an attribute.
    /// </summary>
    public string ModelName => "Quick start demo model";

    public event EventHandler<HassEventArgs>? Event1;

    public event EventHandler<HassEventArgs>? Event2;

    public event EventHandler<HassEventArgs>? Event3;

    public void OnKeyPressed(char keyChar)
    {
        HandleEvent1KeyPresses(keyChar);

        if (keyChar == 'c')
        {
            Event2?.Invoke(this, new HassEventArgs("PressedC"));
        }

        if (keyChar == 'd')
        {
            Event2?.Invoke(this, new HassEventArgs("PressedD"));
        }

        if (keyChar == 'e')
        {
            Event3?.Invoke(this, HassEventArgsFactory.From(EventTypes.KeyPressE));
        }

        if (keyChar == 'f')
        {
            Event3?.Invoke(this, HassEventArgsFactory.From(EventTypes.KeyPressF));
        }
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