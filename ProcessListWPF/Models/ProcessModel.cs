using System;
using System.Diagnostics;

namespace ProcessListWPF.Models;

public class ProcessModel
{
    public event EventHandler<EventArgs>? Changed;

    public int Id { get; set; }
    public string Name { get; set; }
    public int Priority { get; set; }
    public double MemoryBytes { get; set; }
    public double MemoryMB => MemoryBytes / 1048576.0;
    public bool Responding { get; set; }

    // TODO ERRORS

    public ProcessModel()
    {
        Name = string.Empty;
    }

    public ProcessModel(Process process)
    {
        Id = process.Id;
        Name = process.ProcessName;
        Priority = process.BasePriority;
        MemoryBytes = process.WorkingSet64;
        Responding = process.Responding;
    }

    public void Update(Process process)
    {
        Id = process.Id;
        Name = process.ProcessName;
        Priority = process.BasePriority;
        MemoryBytes = process.WorkingSet64;
        Responding = process.Responding;

        Changed?.Invoke(this, new EventArgs());
    }

    public override bool Equals(object? obj)
    {
        return obj is ProcessModel model &&
               Id == model.Id &&
               Name == model.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }
}
