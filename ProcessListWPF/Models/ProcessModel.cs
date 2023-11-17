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
    public List<string> ModulesList { get; set; }
    public List<string> ThreadsList { get; set; }

    public ProcessModel()
    {
        _erroredProperties = new HashSet<string>();
        ModulesList = new List<string>();
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
        TryUpdateProperty(nameof(ModulesList), () =>
        {
            var modules = _process.Modules;
            ModulesList = new List<string>();
            foreach (ProcessModule module in modules)
            {
                var moduleStr = $"{module.ModuleName}";
                ModulesList.Add(moduleStr);
            }
        },
        () => ModulesList = new List<string>() { "Unknown" } );
        TryUpdateProperty(nameof(ThreadsList), () =>
        {
            var threads = _process.Threads;
            ThreadsList = new List<string>();
            foreach (ProcessThread thread in threads)
            {
                var threadStr = $"{thread.Id}";
                ThreadsList.Add(threadStr);
            }
        },
        () => ThreadsList = new List<string>() { "Unknown" });
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
