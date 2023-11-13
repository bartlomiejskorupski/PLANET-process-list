using ProcessListWPF.ViewModels.Shared;
using System;
using System.Diagnostics;
using System.Windows;

namespace ProcessListWPF.ViewModels.Home;

public class DetailsViewModel : ViewModelBase
{
    private int? _id;
    private string? _name;
    private DateTime? _startTime;
    private DateTime? _endTime;
    private string? _status;
    private Visibility _visibility;
    public int? Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string? Name { get => _name; set { _name = value; OnPropertyChanged(); } }
    public DateTime? StartTime { get => _startTime; set { _startTime = value; OnPropertyChanged(); } }
    public DateTime? EndTime { get => _endTime; set { _endTime = value; OnPropertyChanged(); } }
    public string? Status { get => _status; set { _status = value; OnPropertyChanged(); } }
    public Visibility Visibility { get => _visibility; set { _visibility = value; OnPropertyChanged(); } }

    public DetailsViewModel()
    {

    }

    public void SetDetailsFromId(int processId)
    {
        var process = Process.GetProcessById(processId);
        Name = process.ProcessName;
        Id = process.Id;
        try
        {
            StartTime = process.StartTime;
            EndTime = process.ExitTime;
        } catch { }
    }

}
