using ProcessListWPF.Services;
using ProcessListWPF.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProcessListWPF.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public ObservableCollection<ProcessViewModel> ProcessList { get; }

    public HomeViewModel(IRefreshService refreshService)
    {
        ProcessList = new ObservableCollection<ProcessViewModel>();

        refreshService.OnRefreshProcessList += RefreshProcessList;
        RefreshProcessList();
    }


    private async void RefreshProcessList()
    {
        var processes = await Task.Run(GetProcesses);

        ProcessList.Clear();
        foreach (var process in processes)
        {
            ProcessList.Add(process);
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
