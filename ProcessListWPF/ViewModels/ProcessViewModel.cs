using System;
using System.ComponentModel;
using System.Diagnostics;

namespace ProcessListWPF.ViewModels;

public class ProcessViewModel : ViewModelBase
{
    public int Id { get; }
    public string Name { get; }

    public ProcessViewModel()
    {
        
    }

    public ProcessViewModel(Process process)
    {
        Id = process.Id;
        Name = process.ProcessName;
    }


}
