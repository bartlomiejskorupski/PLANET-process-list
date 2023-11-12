using ProcessListWPF.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProcessListWPF.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private ObservableCollection<ProcessViewModel> _processList;
    public ObservableCollection<ProcessViewModel> ProcessList { get => _processList; set { _processList = value; OnPropertyChanged(nameof(ProcessList)); } }
    private ProcessViewModel? _selectedItem;
    public ProcessViewModel? SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }

    public HomeViewModel(IRefreshService refreshService)
    {
        _processList = new ObservableCollection<ProcessViewModel>();

        refreshService.OnRefreshProcessList += RefreshProcessList;
        RefreshProcessList();
    }


    private async void RefreshProcessList()
    {
        var processes = await Task.Run(() => GetProcesses());

        var selected = SelectedItem;
        ProcessList.Clear();

        foreach(var process in processes)
        {
            ProcessList.Add(process);
            if(process.Equals(selected))
                SelectedItem = process;
        }
    }

    private IEnumerable<ProcessViewModel> GetProcesses()
    {
        List<ProcessViewModel> processes = new List<ProcessViewModel>();
        foreach(var process in Process.GetProcesses())
        {
            try
            {
                processes.Add(new ProcessViewModel()
                {
                    Id = process.Id,
                    Name = process.ProcessName,
                    Priority = process.BasePriority,
                    Memory = process.WorkingSet64 / 1048576D

                });
            }
            catch
            {

            }
        }
        return processes;
    }

}
