using ProcessListWPF.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace ProcessListWPF.ViewModels.Home;

public class DetailsViewModel : ViewModelBase
{
    private int _id;
    private string _name;
    private string _startTime;
    private string _status;
    private Visibility _visibility;

    public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
    public string StartTime { get => _startTime; set { _startTime = value; OnPropertyChanged(); } }
    public string Status { get => _status; set { _status = value; OnPropertyChanged(); } }
    public Visibility Visibility { get => _visibility; set { _visibility = value; OnPropertyChanged(); } }

    private HashSet<string> _erroredProperties;

    public DetailsViewModel()
    {
        _name = string.Empty;
        _startTime = string.Empty;
        _status = string.Empty;
        _erroredProperties = new HashSet<string>();
    }

    public void ClearErrors()
    {
        _erroredProperties.Clear();
    }

    public void UpdateDetails(int processId)
    {
        Process process;
        try
        {
            process = Process.GetProcessById(processId);
        }
        catch (Exception)
        {
            return;
        }
        Id = process.Id;
        Name = process.ProcessName;

        if (!_erroredProperties.Contains(nameof(StartTime)))
        {
            try
            {
                StartTime = process.StartTime.ToString("dd.MM.yyyy H:mm:ss");
            } catch 
            {
                StartTime = "Unknown";
                _erroredProperties.Add(nameof(StartTime));
            }
        }

    }

}
