using ProcessListWPF.ViewModels.Shared;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProcessListWPF.Services;

public class ProcessService : IProcessService
{
    public ProcessService()
    {

    }

    public IEnumerable<ProcessViewModel> GetProcessList()
    {
        List<ProcessViewModel> processes = new List<ProcessViewModel>();
        foreach (var process in Process.GetProcesses())
        {
            processes.Add(ProcessToViewModel(process));
        }
        return processes;
    }

    private ProcessViewModel ProcessToViewModel(Process process)
    {
        var pvm = new ProcessViewModel()
        {
            Id = process.Id,
            Name = process.ProcessName,
            Priority = process.BasePriority,
            Memory = process.WorkingSet64 / 1048576D
        };

        pvm.Responding = process.Responding ? "Responding" : "Not responding";

        return pvm;
    }
}
