using System.Collections.ObjectModel;

namespace ProcessListWPF.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public ObservableCollection<ProcessViewModel> ProcessList { get; set; }

    public HomeViewModel()
    {
        ProcessList = new ObservableCollection<ProcessViewModel>()
        {
            new ProcessViewModel() {Id = 1, Name = "Process 1", Description = "Process description", Type = "USER"},
            new ProcessViewModel() {Id = 2, Name = "Process 2", Description = "Process description", Type = "USER"},
            new ProcessViewModel() {Id = 3, Name = "Process 3", Description = "Process description", Type = "SYSTEM"},
            new ProcessViewModel() {Id = 4, Name = "Process 4", Description = "Process description", Type = "SYSTEM"},
        };
    }

}
