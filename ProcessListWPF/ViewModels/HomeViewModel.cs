using ProcessListWPF.Commands;
using ProcessListWPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessListWPF.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private IEnumerable<ProcessViewModel> _unfilteredProcesses;
    private ObservableCollection<ProcessViewModel> _processList;
    public ObservableCollection<ProcessViewModel> ProcessList { get => _processList; set { _processList = value; OnPropertyChanged(nameof(ProcessList)); } }
    private ProcessViewModel? _selectedItem;
    public ProcessViewModel? SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }
    public ICommand KillProcessCommand { get; set; }
    private string _filterTBText;
    public string FilterTBText 
    { 
        get => _filterTBText;
        set
        {
            _filterTBText = value;
            OnPropertyChanged(nameof(FilterTBText));
            FilterTBTextChanged();
        }
    }

    public HomeViewModel(IRefreshService refreshService)
    {
        _unfilteredProcesses = new List<ProcessViewModel>();
        _processList = new ObservableCollection<ProcessViewModel>();
        _filterTBText = "";

        KillProcessCommand = new RelayCommand(_ => KillSelectedProcess(), _ => SelectedItem != null);

        refreshService.OnRefreshProcessList += RefreshProcessList;
        RefreshProcessList();
    }

    private void KillSelectedProcess()
    {
        if (_selectedItem == null)
            return;

        try
        {
            Process.GetProcessById(_selectedItem.Id).Kill();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().Name);
        }
    }

    private void FilterTBTextChanged()
    {
        UpdateProcessList(FilterTBText);
    }

    private async void RefreshProcessList()
    {
        _unfilteredProcesses = await Task.Run(() => GetProcesses());

        UpdateProcessList(FilterTBText);
    }

    private void UpdateProcessList(string? filter = null)
    {
        var selected = SelectedItem;
        ProcessList.Clear();

        var processes = _unfilteredProcesses;
        if (!string.IsNullOrEmpty(filter))
            processes = _unfilteredProcesses.Where(proc => proc.Name?.ToLower().Contains(filter.ToLower()) ?? false);

        foreach (var process in processes)
        {
            ProcessList.Add(process);
            if (process.Equals(selected))
                SelectedItem = process;
        }
    }

    private IEnumerable<ProcessViewModel> GetProcesses()
    {
        List<ProcessViewModel> processes = new List<ProcessViewModel>();
        foreach(var process in Process.GetProcesses())
        {
            try
            {
                processes.Add(new ProcessViewModel()
                {
                    Id = process.Id,
                    Name = process.ProcessName,
                    Priority = process.BasePriority,
                    Memory = process.WorkingSet64 / 1048576D

                });
            }
            catch
            {

            }
        }
        return processes;
    }

}
