using CommunityToolkit.Mvvm.ComponentModel;
using NoeticTools.Net2HassMqtt.Entities.Framework;


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

    public event EventHandler<HassEventArgs>? AbEvent;

    public event EventHandler<HassEventArgs>? CdEvent;

    public event EventHandler<HassEventArgs>? EfEvent;

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
            AbEvent?.Invoke(this, new HassEventArgs("PressedA", aEventAttributes));
        }

        if (keyChar == 'b')
        {
            Console.WriteLine($"\nFiring AB event 'b'.\n");
            AbEvent?.Invoke(this, new HassEventArgs("PressedB"));
        }

        if (keyChar == 'c')
        {
            Console.WriteLine($"\nFiring CD event 'c'.\n");
            CdEvent?.Invoke(this, new HassEventArgs("PressedC"));
        }

        if (keyChar == 'd')
        {
            Console.WriteLine($"\nFiring CD event 'd'.\n");
            CdEvent?.Invoke(this, new HassEventArgs("PressedD"));
        }

        if (keyChar == 'e')
        {
            Console.WriteLine($"\nFiring EF event 'e'.\n");
            EfEvent?.Invoke(this, HassEventArgs.From(Event3Types.KeyPressE));
        }

        if (keyChar == 'f')
        {
            Console.WriteLine($"\nFiring EF event 'f'.\n");
            EfEvent?.Invoke(this, HassEventArgs.From(Event3Types.KeyPressF));
        }
    }
}
