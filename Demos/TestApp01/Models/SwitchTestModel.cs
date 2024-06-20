using CommunityToolkit.Mvvm.ComponentModel;


namespace NoeticTools.TestApp01.Models;

internal partial class SwitchTestModel : ObservableObject
{
    [ObservableProperty]
    private bool _switch1 = true;

    [ObservableProperty]
    private bool _switch2;
}