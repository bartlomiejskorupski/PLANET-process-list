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
    private bool _isRefreshFast;
    public bool IsRefreshFast
    {
        get => _isRefreshFast;
        set
        {
            _isRefreshFast = value;
            OnPropertyChanged(nameof(IsRefreshFast));
        }
    }
    private bool _isRefreshNormal;
    public bool IsRefreshNormal
    {
        get => _isRefreshNormal;
        set
        {
            _isRefreshNormal = value;
            OnPropertyChanged(nameof(IsRefreshNormal));
        }
    }
    private bool _isRefreshSlow;
    public bool IsRefreshSlow
    {
        get => _isRefreshSlow;
        set
        {
            _isRefreshSlow = value;
            OnPropertyChanged(nameof(IsRefreshSlow));
        }
    }
    private bool _isRefreshPaused;
    public bool IsRefreshPaused
    {
        get => _isRefreshPaused;
        set
        {
            _isRefreshPaused = value;
            OnPropertyChanged(nameof(IsRefreshPaused));
        }
    }
    public ICommand ExitCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand RefreshFastCommand { get; }
    public ICommand RefreshNormalCommand { get; }
    public ICommand RefreshSlowCommand { get; }
    public ICommand RefreshPausedCommand { get; }
    private readonly DispatcherTimer _refreshTimer;
    public static readonly TimeSpan REFRESH_FAST = TimeSpan.FromSeconds(0.5);
    public static readonly TimeSpan REFRESH_NORMAL = TimeSpan.FromSeconds(1);
    public static readonly TimeSpan REFRESH_SLOW = TimeSpan.FromSeconds(5);

    public HomeViewModel()
    {
        ProcessList = new ObservableCollection<ProcessViewModel>();

        _refreshTimer = new DispatcherTimer();
        _refreshTimer.Tick += RefreshTimerTick;
        SetRefreshNormal();
        _refreshTimer.Start();

        ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        RefreshCommand = new RelayCommand(_ => RefreshProcessList());
        RefreshFastCommand = new RelayCommand(_ => SetRefreshFast());
        RefreshNormalCommand = new RelayCommand(_ => SetRefreshNormal());
        RefreshSlowCommand = new RelayCommand(_ => SetRefreshSlow());
        RefreshPausedCommand = new RelayCommand(_ => RefreshPausedClick());

        RefreshProcessList();
    }

    private void SetRefreshFast()
    {
        _refreshTimer.Interval = REFRESH_FAST;
        IsRefreshFast = true;
        IsRefreshNormal = false;
        IsRefreshSlow = false;
    }

    private void SetRefreshNormal()
    {
        _refreshTimer.Interval = REFRESH_NORMAL;
        IsRefreshFast = false;
        IsRefreshNormal = true;
        IsRefreshSlow = false;
    }

    private void SetRefreshSlow()
    {
        _refreshTimer.Interval = REFRESH_SLOW;
        IsRefreshFast = false;
        IsRefreshNormal = false;
        IsRefreshSlow = true;
    }

    private void RefreshPausedClick()
    {
        if (_refreshTimer.IsEnabled)
        {
            _refreshTimer.Stop();
            IsRefreshPaused = true;
        }
        else
        {
            _refreshTimer.Start();
            IsRefreshPaused = false;
        }
    }

    private void RefreshTimerTick(object? obj, EventArgs e)
    {
        _refreshTimer.Stop();
        RefreshProcessList();
        _refreshTimer.Start();
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

}
