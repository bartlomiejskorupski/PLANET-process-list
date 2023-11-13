using ProcessListWPF.ViewModels.Shared;
using System.Collections.Generic;

namespace ProcessListWPF.Services
{
    public interface IProcessService
    {
        IEnumerable<ProcessViewModel> GetProcessList();
    }
}