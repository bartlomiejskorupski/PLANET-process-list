using ProcessListWPF.Commands;
using ProcessListWPF.Services;
using ProcessListWPF.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessListWPF.ViewModels.Home;

public class HomeViewModel : ViewModelBase
{
    private IProcessService _processService;
    private IEnumerable<ProcessViewModel> _unfilteredProcesses;

    private ObservableCollection<ProcessViewModel> _processList;
    public ObservableCollection<ProcessViewModel> ProcessList { get => _processList; set { _processList = value; OnPropertyChanged(nameof(ProcessList)); } }

    private ProcessViewModel? _selectedItem;
    public ProcessViewModel? SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }

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

    public ICommand KillProcessCommand { get; set; }

    public HomeViewModel(IRefreshService refreshService, IProcessService processService) 
    {
        _processService = processService;
        _unfilteredProcesses = new List<ProcessViewModel>();
        _processList = new ObservableCollection<ProcessViewModel>();
        _filterTBText = "";

        KillProcessCommand = new RelayCommand(_ => KillSelectedProcess(), _ => SelectedItem != null);

        refreshService.OnRefreshProcessList += RefreshProcessList;
        RefreshProcessList();
    }

    private void KillSelectedProcess()
    {
        if (_selectedItem == null) return;

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
        _unfilteredProcesses = await Task.Run(() => _processService.GetProcessList());

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

}
