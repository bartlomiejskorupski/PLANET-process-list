using System;
using System.Diagnostics;
using ProcessListWPF.Core;

namespace ProcessListWPF.ViewModels.Shared;

public class ProcessViewModel : ViewModelBase
{
    private int _id;
    private string _name;
    private int _priority;
    private double _memory;
    private string _responding;

    public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
    public int Priority { get => _priority; set { _priority = value; OnPropertyChanged(); } }
    public double Memory { get => _memory; set { _memory = value; OnPropertyChanged(); OnPropertyChanged(nameof(MemoryFormatted)); } }
    public string MemoryFormatted => $"{Memory:0.0} MB";
    public string Responding { get => _responding; set { _responding = value; OnPropertyChanged(); } }
    public ProcessViewModel()
    {
        _name = string.Empty;
        _responding = string.Empty;
    }

    public ProcessViewModel(Process process) : this()
    {
        SetFieldsFromProcess(process);
    }

    public void SetFieldsFromProcess(Process process)
    {
        Id = process.Id;
        Name = process.ProcessName;
        Priority = process.BasePriority;
        Memory = process.WorkingSet64 / 1048576D;
        Responding = process.Responding ? "Responding" : "Not responding";
    }

    public void SetFieldsFromPVM(ProcessViewModel other)
    {
        Id = other.Id;
        Name = other.Name;
        Priority = other.Priority;
        Memory = other.Memory;
        Responding = other.Responding;
    }

    public override bool Equals(object? obj)
    {
        return obj is ProcessViewModel model &&
               Id == model.Id &&
               Name == model.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }

}
