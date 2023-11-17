using ProcessListWPF.Core;
using ProcessListWPF.Models;
using System.Diagnostics;

namespace ProcessListWPF.ViewModels.Shared;

public class ProcessViewModel : ViewModelBase
{
    public ProcessModel? Model { get; set; }

    private int _id;
    private string _name;
    private int _priority;
    private double _memory;
    private string _responding;
    private string _cmdLine;

    public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
    public int Priority { get => _priority; set { _priority = value; OnPropertyChanged(); } }
    public string PriorityFormatted => GetFormattedPriority();
    public double Memory { get => _memory; set { _memory = value; OnPropertyChanged(); OnPropertyChanged(nameof(MemoryFormatted)); } }
    public string MemoryFormatted => $"{Memory:0.0} MB";
    public string Responding { get => _responding; set { _responding = value; OnPropertyChanged(); } }

    public string CmdLine
    {
        get { return _cmdLine; }
        set { _cmdLine = value; OnPropertyChanged(); }
    }

    public ProcessViewModel()
    {
        _name = string.Empty;
        _responding = string.Empty;
        _cmdLine = string.Empty;
    }

    public ProcessViewModel(ProcessModel model) : this()
    {
        Update(model);
    }

    public void Update(ProcessModel model)
    {
        Model = model;
        Id = model.Id;
        Name = model.Name;
        Priority = GetPriorityOrdered(model.Priority);
        Memory = model.MemoryMB;
        Responding = model.Responding ? "Responding" : "Not responding";
    }

    public int GetPriorityOrdered(ProcessPriorityClass priorityClass)
    {
        switch (priorityClass)
        {
            case ProcessPriorityClass.Idle: return 0;
            case ProcessPriorityClass.BelowNormal: return 1;
            case ProcessPriorityClass.Normal: return 2;
            case ProcessPriorityClass.AboveNormal: return 3;
            case ProcessPriorityClass.High: return 4;
            case ProcessPriorityClass.RealTime: return 5;
        }
        return -1;
    }

    public string GetFormattedPriority()
    {
        switch (Priority)
        {
            case 0: return "Idle";
            case 1: return "BelowNormal";
            case 2: return "Normal";
            case 3: return "AboveNormal";
            case 4: return "High";
            case 5: return "RealTime";
        }
        return "unknown";
    }

}
