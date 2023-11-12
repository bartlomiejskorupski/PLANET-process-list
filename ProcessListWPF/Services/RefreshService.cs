using System;

namespace ProcessListWPF.Services;

public class RefreshService : IRefreshService
{
    public event Action? OnRefreshProcessList;

    public RefreshService()
    {
        
    }

    public void RequestRefresh()
    {
        OnRefreshProcessList?.Invoke();
    }
}
