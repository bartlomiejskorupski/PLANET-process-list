using ProcessListWPF.Core;
using ProcessListWPF.Models;
using System.Diagnostics;

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
    public string PriorityFormatted => GetFormattedPriority();
    public double Memory { get => _memory; set { _memory = value; OnPropertyChanged(); OnPropertyChanged(nameof(MemoryFormatted)); } }
    public string MemoryFormatted => $"{Memory:0.0} MB";
    public string Responding { get => _responding; set { _responding = value; OnPropertyChanged(); } }
    public ProcessViewModel()
    {
        _name = string.Empty;
        _responding = string.Empty;
    }

    public ProcessViewModel(ProcessModel model) : this()
    {
        Update(model);
    }

    public void Update(ProcessModel model)
    {
        Id = model.Id;
        Name = model.Name;
        Priority = model.Priority;
        Memory = model.MemoryMB;
        Responding = model.Responding ? "Responding" : "Not responding";
    }

    public string GetFormattedPriority()
    {
        var priority = "";

        switch (Priority)
        {
            case 0: priority = "Lowest"; break;
            case 4: priority = "Idle"; break;
            case 6: priority = "Below Normal"; break;
            case 8: priority = "Normal"; break;
            case 9: priority = "Above Normal"; break;
            case 10: priority = "High"; break;
            case 11: priority = "Time Critical"; break;
            case 13: priority = "Highest"; break;
            case 24: priority = "Real Time"; break;
            default: priority = Priority.ToString(); break;
        }

        return priority;
    }

}
