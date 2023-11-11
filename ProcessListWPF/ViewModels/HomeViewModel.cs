using ProcessListWPF.Commands;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        ExitCommand = new RelayCommand(_ => { Application.Current.Shutdown(); });
        RefreshCommand = new RelayCommand(RefreshProcessList);

        RefreshProcessList();
        _refreshTimer = new DispatcherTimer();
        _refreshTimer.Interval = TimeSpan.FromSeconds(1);
        _refreshTimer.Tick += (_, _) => RefreshProcessList();
        _refreshTimer.Start();
    }

    public void RefreshProcessList(object? parameter = null)
    {
        ProcessList.Clear();
        var processes = Process.GetProcesses();
        foreach (var process in processes)
        { 
            ProcessList.Add(new ProcessViewModel(process));
        }
    }

}
