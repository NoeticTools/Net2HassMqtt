using CommunityToolkit.Mvvm.ComponentModel;


namespace NoeticTools.TestApp01.Models;

internal partial class SensorTestModel : ObservableObject
{
    [ObservableProperty]
    private bool _carbonMonoxideStatus;

    [ObservableProperty]
    private double _current = 11.1;

    [ObservableProperty]
    private DateOnly _dateOnlyDate = new(2026, 12, 31);

    [ObservableProperty]
    private DateTimeOffset _dateTimeOffsetTimestamp = new(2026, 12, 31, 2, 59, 29, TimeSpan.FromHours(10));

    [ObservableProperty]
    private DateTime _dateTimeTimestamp = new(2026, 12, 31, 3, 59, 30);

    [ObservableProperty]
    private int _level1 = 10;

    [ObservableProperty]
    private double _level2 = 10.1;

    [ObservableProperty]
    private double _temperature = 42.0;

    public void CurrentCommandHandler(double value)
    {
        Console.WriteLine($"Current commanded to: {value}");
        Current = value;
    }

    public void Level1CommandHandler(int value)
    {
        Console.WriteLine($"Level 1 commanded to: {value}");
        Level1 = value;
    }

    public void TemperatureCommandHandler(double value)
    {
        Console.WriteLine($"Temperature commanded to: {value}");
        Temperature = value;
    }
}