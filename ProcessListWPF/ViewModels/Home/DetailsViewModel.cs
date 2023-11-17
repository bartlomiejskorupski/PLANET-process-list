using ProcessListWPF.Core;
using ProcessListWPF.Models;
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
    private string _memory;

    public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
    public string StartTime { get => _startTime; set { _startTime = value; OnPropertyChanged(); } }
    public string Status { get => _status; set { _status = value; OnPropertyChanged(); } }
    public Visibility Visibility { get => _visibility; set { _visibility = value; OnPropertyChanged(); } }
    public string Memory { get => _memory; set { _memory = value; OnPropertyChanged(); } }
    private string _location;

    public string Location
    {
        get { return _location; }
        set { _location = value; OnPropertyChanged(); }
    }


    public DetailsViewModel()
    {
        _name = string.Empty;
        _startTime = string.Empty;
        _status = string.Empty;
        _memory = string.Empty;
        _location = string.Empty;
    }

    public void UpdateDetails(ProcessModel model)
    {
        Id = model.Id;
        Name = model.Name;
        Memory = $"{model.MemoryMB:0.0} MB (bytes: {model.MemoryBytes})";

        model.UpdateDetails();
        if(model.StartTime is DateTime startTime)
        {
            StartTime = startTime.ToString("dd.MM.yyyy HH:mm:ss");
        }
        else
        {
            StartTime = "Unknown";
        }
        Location = model.Location;
    }

}
