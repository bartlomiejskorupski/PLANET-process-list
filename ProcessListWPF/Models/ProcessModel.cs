using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProcessListWPF.Models;

public class ProcessModel
{
    private Process? _process;

    public int Id { get; set; }
    public string Name { get; set; }
    public ProcessPriorityClass Priority { get; set; }
    public double MemoryBytes { get; set; }
    public double MemoryMB => MemoryBytes / 1048576.0;
    public bool Responding { get; set; }
    public DateTime? StartTime { get; set; }
    public string Location { get; set; }

    private HashSet<string> _erroredProperties;
    public bool UpdatedFlag { get; set; }

    public ProcessModel()
    {
        _erroredProperties = new HashSet<string>();
        Name = string.Empty;
        UpdatedFlag = false;
        Location = string.Empty;
    }

    public ProcessModel(Process process) : this()
    {
        Update(process);
    }

    public void Update(Process process)
    {
        _process = process; 
        Id = process.Id;
        Name = process.ProcessName;
        MemoryBytes = process.WorkingSet64;
        Responding = process.Responding;

        TryUpdateProperty(nameof(Priority), () => Priority = process.PriorityClass, () => Priority = ProcessPriorityClass.Idle);
        TryUpdateProperty(nameof(Location), () => Location = process.MainModule!.FileName, () => Location = "Unknown");
    }

    public void UpdateDetails()
    {
        if (_process == null) return;

        TryUpdateProperty(nameof(StartTime), () => StartTime = _process.StartTime);
    }

    private void TryUpdateProperty(string propertyName, Action updatePropertyAction, Action? actionOnError = null)
    {
        if (!_erroredProperties.Contains(propertyName))
        {
            try
            {
                updatePropertyAction();
            }
            catch
            {
                actionOnError?.Invoke();
                _erroredProperties.Add(propertyName);
            }
        }
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
