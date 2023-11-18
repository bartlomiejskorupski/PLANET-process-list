using ProcessListWPF.Commands;
using ProcessListWPF.Core;
using ProcessListWPF.Models;
using ProcessListWPF.Services;
using ProcessListWPF.ViewModels.Shared;
using ProcessListWPF.Views.Shared;
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
    private ProcessModelCollection _processCollection;
    private IDialogFactory<ChangePriorityWindow> _changePriorityDialogFactory;
    
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
    public ICommand ChangePriorityCommand { get; set; }

    public HomeViewModel(IRefreshService refreshService, DetailsViewModel detailsViewModel, IDialogFactory<ChangePriorityWindow> changePriorityDialogFactory) 
    {
        _detailsViewModel = detailsViewModel;
        _changePriorityDialogFactory = changePriorityDialogFactory;
        DetailsVisibility = Visibility.Collapsed;
        _processList = new ObservableCollection<ProcessViewModel>();
        _processCollection = new ProcessModelCollection();
        _filterTBText = "";

        KillProcessCommand = new RelayCommand(_ => KillSelectedProcess(), _ => SelectedItem != null);
        ChangePriorityCommand = new RelayCommand(_ => ChangeSelectedPriority(), _ => SelectedItem != null);

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
        DetailsViewModel.UpdateDetails(SelectedItem.Model!);
        DetailsVisibility = Visibility.Visible;

    }

    private void ChangeSelectedPriority()
    {
        if (SelectedItem == null || SelectedItem.Model == null)
            return;

        var priorityWindow = _changePriorityDialogFactory.Create();
        priorityWindow.SetDefaultPriority(SelectedItem.Model.Priority);
        var success = priorityWindow.ShowDialog();
        if (success == false) return;

        var chosenPriority = priorityWindow.ChosenPriority;
        var changedSuccessfully = SelectedItem.Model.ChangePriority(chosenPriority);
        if (!changedSuccessfully)
            MessageBox.Show("Failed to change priority.\nAccess denied.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

    }

    private void KillSelectedProcess()
    {
        if (_selectedItem == null) return;

        try
        {
            Process.GetProcessById(_selectedItem.Id).Kill();
        }
        catch
        {
            MessageBox.Show("Failed to kill process.\nAccess denied.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void FilterTBTextChanged()
    {
        UpdateProcessList();
        FilterProcessList(FilterTBText);
    }

    private void RefreshProcessList()
    {
        _processCollection.Update();
        UpdateProcessList();
        FilterProcessList(FilterTBText);
        if (DetailsVisibility == Visibility.Visible && SelectedItem is ProcessViewModel pvm)
            DetailsViewModel.UpdateDetails(pvm.Model!);
    }

    private void UpdateProcessList()
    {
        foreach (var id in _processCollection.Ids)
        {
            var processModel = _processCollection.GetProcessModel(id);
            var pvm = ProcessList.Where(pvm => pvm.Id == id).FirstOrDefault();
            if (pvm != null)
            {
                pvm.Update(processModel);
                continue;
            }

            ProcessList.Add(new ProcessViewModel(processModel));
        }

        var killedProcesses = ProcessList.Where(pvm => !_processCollection.Contains(pvm.Id));
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
