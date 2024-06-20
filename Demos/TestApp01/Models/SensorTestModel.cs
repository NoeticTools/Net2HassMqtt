using CommunityToolkit.Mvvm.ComponentModel;


namespace NoeticTools.TestApp01.Models;

internal partial class SensorTestModel : ObservableObject
{
    [ObservableProperty]
    private bool _carbonMonoxideStatus;

    [ObservableProperty]
    private double _current = 11.1;

    [ObservableProperty]
    private int _level1 = 10;

    [ObservableProperty]
    private double _level2 = 10.1;

    [ObservableProperty]
    private double _temperature = 42.0;

    public void Level1CommandHandler(int value)
    {
        Console.WriteLine($"Level 1 commanded to: {value}");
        Level1 = value;
    }

    public void CurrentCommandHandler(double value)
    {
        Console.WriteLine($"Current commanded to: {value}");
        Current = value;
    }

    public void TemperatureCommandHandler(double value)
    {
        Console.WriteLine($"Temperature commanded to: {value}");
        Temperature = value;
    }
}