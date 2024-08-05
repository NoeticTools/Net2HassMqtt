using CommunityToolkit.Mvvm.ComponentModel;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using static NoeticTools.Net2HassMqtt.QuickStartDemoApp.SampleEntityModels.QuickStartDemoModel;


namespace NoeticTools.Net2HassMqtt.QuickStartDemoApp.SampleEntityModels;

public partial class QuickStartDemoModel : ObservableObject
{
    [ObservableProperty] private bool _batteryCharging;

    public enum Event3Types
    {
        KeyPressE = 0,
        KeyPressF = 1
    }

    /// <summary>
    /// Property that will be automatically read on every Event1 event and added as an attribute.
    /// </summary>
    public string ModelName => "Quick start demo model";

    public event EventHandler<HassEventArgs>? ABEvent;

    public event EventHandler<HassEventArgs>? CDEvent;

    public event EventHandler<HassEventArgs>? EFEvent;

    public void OnKeyPressed(char keyChar)
    {
        if (keyChar == 'a')
        {
            Console.WriteLine($"\nFiring AB event 'a'.\n");
            var aEventAttributes = new Dictionary<string, string>()
            {
                {"From AB event attribute 1", "An example attribute value defined in the client model."},
                {"From AB event attribute 2", "Another attribute value set when the event is fired in app code."},
            };
            ABEvent?.Invoke(this, new HassEventArgs("PressedA", aEventAttributes));
        }

        if (keyChar == 'b')
        {
            Console.WriteLine($"\nFiring AB event 'b'.\n");
            ABEvent?.Invoke(this, new HassEventArgs("PressedB"));
        }

        if (keyChar == 'c')
        {
            Console.WriteLine($"\nFiring CD event 'c'.\n");
            CDEvent?.Invoke(this, new HassEventArgs("PressedC"));
        }

        if (keyChar == 'd')
        {
            Console.WriteLine($"\nFiring CD event 'd'.\n");
            CDEvent?.Invoke(this, new HassEventArgs("PressedD"));
        }

        if (keyChar == 'e')
        {
            Console.WriteLine($"\nFiring EF event 'e'.\n");
            EFEvent?.Invoke(this, HassEventArgs.From(Event3Types.KeyPressE));
        }

        if (keyChar == 'f')
        {
            Console.WriteLine($"\nFiring EF event 'f'.\n");
            EFEvent?.Invoke(this, HassEventArgs.From(Event3Types.KeyPressF));
        }
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