using CommunityToolkit.Mvvm.ComponentModel;


namespace Examples.SampleEntityModels
{
    internal partial class EnvironmentSensorModel : ObservableObject
    {
        [ObservableProperty] private bool _temperature;
        [ObservableProperty] private bool _humidity;
    }
}
