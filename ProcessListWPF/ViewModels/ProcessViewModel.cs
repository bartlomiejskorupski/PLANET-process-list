using System;

namespace ProcessListWPF.ViewModels;

public class ProcessViewModel : ViewModelBase
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Priority { get; set; }
    public double? Memory { get; set; }
    public string MemoryFormatted => $"{Memory:0.0} MB";
    public ProcessViewModel()
    {
        
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
