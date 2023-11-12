using System;

namespace ProcessListWPF.Services;

public interface IRefreshService
{
    public event Action OnRefreshProcessList;
    void RequestRefresh();
}
