using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProcessListWPF.Models;

public class ProcessModelCollection
{
    private Dictionary<int, ProcessModel> _processes;

    public IEnumerable<int> Ids => _processes.Keys;

    public ProcessModelCollection()
    {
        _processes = new Dictionary<int, ProcessModel>();
    }

    public ProcessModel GetProcessModel(int id) => _processes[id]; 

    public bool Contains(int id) => _processes.ContainsKey(id);

    public void Update()
    {
        var systemProcesses = Process.GetProcesses();

        // Add new and update
        foreach (var systemProcess in systemProcesses)
        {
            if (_processes.ContainsKey(systemProcess.Id))
            {
                _processes[systemProcess.Id].Update(systemProcess);
            }
            else
            {
                _processes[systemProcess.Id] = new ProcessModel(systemProcess);
            }
            _processes[systemProcess.Id].UpdatedFlag = true;
        }

        // Delete killed processes
        foreach (var key in _processes.Keys)
        {
            var process = _processes[key];
            if(process.UpdatedFlag)
            {
                process.UpdatedFlag = false;
            }
            else
            {
                _processes.Remove(key);
            }
        }

    }

}
