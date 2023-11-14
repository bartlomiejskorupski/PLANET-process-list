using ProcessListWPF.Core;
using ProcessListWPF.ViewModels.Home;

namespace ProcessListWPF.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase? _currentViewModel;
    public ViewModelBase? CurrentViewModel
    { 
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    public MainViewModel(HomeViewModel homeViewModel)
    {
        CurrentViewModel = homeViewModel;
    }
}
