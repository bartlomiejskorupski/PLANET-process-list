using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProcessListWPF.Models;

public class ProcessModel
{
    public event EventHandler<EventArgs>? Changed;

    public int Id { get; set; }
    public string Name { get; set; }
    public ProcessPriorityClass Priority { get; set; }
    public double MemoryBytes { get; set; }
    public double MemoryMB => MemoryBytes / 1048576.0;
    public bool Responding { get; set; }
    public string CmdLine { get; set; }

    private HashSet<string> _erroredProperties;
    public bool UpdatedFlag { get; set; }

    public ProcessModel()
    {
        _erroredProperties = new HashSet<string>();
        Name = string.Empty;
        CmdLine = string.Empty;
        UpdatedFlag = false;
    }

    public ProcessModel(Process process) : this()
    {
        Update(process);
    }

    public void Update(Process process)
    {
        Id = process.Id;
        Name = process.ProcessName;
        MemoryBytes = process.WorkingSet64;
        Responding = process.Responding;

        if (!_erroredProperties.Contains(nameof(Priority)))
        {
            try
            {
                Priority = process.PriorityClass;
            }
            catch
            {
                Priority = ProcessPriorityClass.Idle;
                _erroredProperties.Add(nameof(Priority));
            }
        }

        if (!_erroredProperties.Contains(nameof(CmdLine)))
        {
            try
            {
                CmdLine = process.MainModule!.FileName;
            }
            catch
            {
                CmdLine = "Unknown";
                _erroredProperties.Add(nameof(CmdLine));
            }
        }

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
