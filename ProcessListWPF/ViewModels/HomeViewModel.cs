using ProcessListWPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProcessListWPF.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public ObservableCollection<ProcessViewModel> ProcessList { get; }
    public ICommand ExitCommand { get; }
    public ICommand RefreshCommand { get; }
    private readonly DispatcherTimer _refreshTimer;

    public HomeViewModel()
    {
        ProcessList = new ObservableCollection<ProcessViewModel>();

        ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        RefreshCommand = new RelayCommand(_ => RefreshProcessList());

        RefreshProcessList();
        _refreshTimer = new DispatcherTimer();
        _refreshTimer.Interval = TimeSpan.FromSeconds(1);
        _refreshTimer.Tick += RefreshTimerTick;
        _refreshTimer.Start();
    }

    public async void RefreshProcessList()
    {
        var processes = await Task.Run(GetProcesses);

        ProcessList.Clear();
        foreach (var process in processes)
        {
            ProcessList.Add(process);
        }
    }

    public IEnumerable<ProcessViewModel> GetProcesses()
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
                    Priority = process.BasePriority.ToString(),
                    Memory = process.WorkingSet64 / 1048576D

                });
            }
            catch
            {

            }
        }
        return processes;
    }

    public void RefreshTimerTick(object? obj, EventArgs e)
    {
        _refreshTimer.Stop();
        RefreshProcessList();
        _refreshTimer.Start();
    }

}
