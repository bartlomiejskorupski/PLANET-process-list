using ProcessListWPF.Commands;
using ProcessListWPF.Core;
using ProcessListWPF.Services;
using ProcessListWPF.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProcessListWPF.ViewModels.Home;

public class HomeViewModel : ViewModelBase
{
    private List<ProcessViewModel> _unfiltededProcesses;
    private ObservableCollection<ProcessViewModel> _processList;
    private ProcessViewModel? _selectedItem;
    private DetailsViewModel _detailsViewModel;
    private Visibility _detailsVisibility;

    public ObservableCollection<ProcessViewModel> ProcessList { get => _processList; set { _processList = value; OnPropertyChanged(); } }
    public ProcessViewModel? SelectedItem 
    {
        get => _selectedItem;
        set 
        {
            _selectedItem = value; 
            OnPropertyChanged();
            OnSelectedItemChanged();
        } 
    }
    public DetailsViewModel DetailsViewModel { get => _detailsViewModel; set { _detailsViewModel = value; OnPropertyChanged(); } }
    public Visibility DetailsVisibility 
    {
        get => _detailsVisibility;
        set 
        { 
            _detailsVisibility = value;
            OnPropertyChanged();
            DetailsViewModel.Visibility = value;
        }
    }

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

    public HomeViewModel(IRefreshService refreshService, DetailsViewModel detailsViewModel) 
    {
        _detailsViewModel = detailsViewModel;
        DetailsVisibility = Visibility.Collapsed;
        _processList = new ObservableCollection<ProcessViewModel>();
        _unfiltededProcesses = new List<ProcessViewModel>();
        _filterTBText = "";

        KillProcessCommand = new RelayCommand(_ => KillSelectedProcess(), _ => SelectedItem != null);

        refreshService.OnRefreshProcessList += RefreshProcessList;
        RefreshProcessList();
    }

    private void OnSelectedItemChanged()
    {
        if(SelectedItem == null)
        {
            DetailsVisibility = Visibility.Collapsed;
            return;
        }
        DetailsViewModel.Error = false;
        DetailsViewModel.SetDetailsFromId(SelectedItem.Id);
        DetailsVisibility = Visibility.Visible;

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
        UpdateProcessList(_unfiltededProcesses);
        FilterProcessList(FilterTBText);
    }

    private void RefreshProcessList()
    {
        _unfiltededProcesses = Process.GetProcesses().Select(p => new ProcessViewModel(p)).ToList();
        UpdateProcessList(_unfiltededProcesses);
        FilterProcessList(FilterTBText);
        if (DetailsVisibility == Visibility.Visible && SelectedItem is ProcessViewModel pvm)
            DetailsViewModel.SetDetailsFromId(pvm.Id);
    }

    private void UpdateProcessList(IEnumerable<ProcessViewModel> processes)
    {
        foreach (var process in processes)
        {
            var pvm = ProcessList.Where(pvm => pvm.Id == process.Id).FirstOrDefault();
            if (pvm != null)
            {
                pvm.SetFieldsFromPVM(process);
                continue;
            }

            ProcessList.Add(process);
        }

        var currProcessIds = processes.Select(p => p.Id);
        var killedProcesses = ProcessList.Where(pvm => !currProcessIds.Contains(pvm.Id));
        for (int i = killedProcesses.Count() - 1; i >= 0; i--)
        {
            var killed = killedProcesses.ElementAt(i);
            ProcessList.Remove(killed);
        }
    }

    private void FilterProcessList(string filter)
    {
        if (string.IsNullOrEmpty(filter))
            return;

        for(int i = ProcessList.Count - 1; i >= 0; i--)
        {
            var process = ProcessList[i];
            if (!process.Name?.ToLower().Contains(filter.ToLower()) ?? true)
                ProcessList.Remove(process);
        }

        
    }

}
