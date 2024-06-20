using CommunityToolkit.Mvvm.ComponentModel;


namespace NoeticTools.Net2HassMqtt.Examples1.SampleEntityModels;

internal partial class EnvironmentSensorModel : ObservableObject
{
    [ObservableProperty]
    private int _humidity;

    [ObservableProperty]
    private int _temperature;

    [ObservableProperty]
    private bool _testBool;
}