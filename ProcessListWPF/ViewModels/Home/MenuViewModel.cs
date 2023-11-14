using System.Windows.Input;
using System.Windows.Threading;
using System;
using ProcessListWPF.Commands;
using ProcessListWPF.Services;
using System.Windows;
using ProcessListWPF.Core;

namespace ProcessListWPF.ViewModels.Home;

public class MenuViewModel : ViewModelBase
{
    private readonly IRefreshService _refreshService;

    private readonly DispatcherTimer _refreshTimer;
    public static readonly TimeSpan REFRESH_FAST = TimeSpan.FromSeconds(0.5);
    public static readonly TimeSpan REFRESH_NORMAL = TimeSpan.FromSeconds(1);
    public static readonly TimeSpan REFRESH_SLOW = TimeSpan.FromSeconds(5);

    private bool _isRefreshFast;
    public bool IsRefreshFast
    {
        get => _isRefreshFast;
        set
        {
            _isRefreshFast = value;
            OnPropertyChanged();
        }
    }
    private bool _isRefreshNormal;
    public bool IsRefreshNormal
    {
        get => _isRefreshNormal;
        set
        {
            _isRefreshNormal = value;
            OnPropertyChanged();
        }
    }
    private bool _isRefreshSlow;
    public bool IsRefreshSlow
    {
        get => _isRefreshSlow;
        set
        {
            _isRefreshSlow = value;
            OnPropertyChanged();
        }
    }
    private bool _isRefreshPaused;
    public bool IsRefreshPaused
    {
        get => _isRefreshPaused;
        set
        {
            _isRefreshPaused = value;
            OnPropertyChanged();
        }
    }
    public ICommand ExitCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand RefreshFastCommand { get; }
    public ICommand RefreshNormalCommand { get; }
    public ICommand RefreshSlowCommand { get; }
    public ICommand RefreshPausedCommand { get; }

    public MenuViewModel(IRefreshService refreshService)
    {
        _refreshService = refreshService;

        _refreshTimer = new DispatcherTimer();
        _refreshTimer.Tick += RefreshTimerTick;
        SetRefreshNormal();
        _refreshTimer.Start();

        ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        RefreshCommand = new RelayCommand(_ => _refreshService.RequestRefresh());
        RefreshFastCommand = new RelayCommand(_ => SetRefreshFast());
        RefreshNormalCommand = new RelayCommand(_ => SetRefreshNormal());
        RefreshSlowCommand = new RelayCommand(_ => SetRefreshSlow());
        RefreshPausedCommand = new RelayCommand(_ => RefreshPausedClick());
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
        _refreshService.RequestRefresh();
        _refreshTimer.Start();
    }
}
