using CommunityToolkit.Mvvm.ComponentModel;
using NoeticTools.TestApp01.Exceptions;


namespace NoeticTools.TestApp01.Models;

internal partial class BinarySensorTestModel : ObservableObject
{
    [ObservableProperty]
    private bool _batteryStatus = true;

    [ObservableProperty]
    private bool _carbonMonoxideStatus;

    [ObservableProperty]
    private bool _doorStatus = true;

    [ObservableProperty]
    private bool _garageDoorStatus;

    [ObservableProperty]
    private bool _status1 = true;

    [ObservableProperty]
    private bool _status2;

    public void BatteryStatusCommand(string command)
    {
        if (command == "ON")
        {
            BatteryStatus = true;
        }
        else if (command == "OFF")
        {
            BatteryStatus = false;
        }
        else
        {
            throw new InvalidCommandException($"Invalid command {command}");
        }

        Console.WriteLine($"BinarySensorTestModel Battery Status set to {Status1}");
    }

    public void DoorCommand(bool command)
    {
        DoorStatus = command;
        Console.WriteLine($"BinarySensorTestModel Door set to {command}");
    }
}