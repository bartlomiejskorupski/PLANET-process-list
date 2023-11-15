using ProcessListWPF.Core;
using ProcessListWPF.Models;

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

}
