using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
namespace ProcessListWPF.Views.Shared;

public partial class ChangePriorityWindow : Window
{
    public ProcessPriorityClass ChosenPriority { get; set; }
    private List<PriorityViewModel> _priorityList;

    public ChangePriorityWindow()
    {
        InitializeComponent();
        _priorityList = new List<PriorityViewModel>() {
            new PriorityViewModel("Real Time", ProcessPriorityClass.RealTime),
            new PriorityViewModel("High", ProcessPriorityClass.High),
            new PriorityViewModel("Above Normal", ProcessPriorityClass.AboveNormal),
            new PriorityViewModel("Normal", ProcessPriorityClass.Normal),
            new PriorityViewModel("Below Normal", ProcessPriorityClass.BelowNormal),
            new PriorityViewModel("Idle", ProcessPriorityClass.Idle)
        };
        PriorityComboBox.ItemsSource = _priorityList;
        PriorityComboBox.SelectedIndex = 3;
    }

    public void SetDefaultPriority(ProcessPriorityClass priority)
    {
        var priorityVM = _priorityList.Where(prio => prio.Priority == priority).FirstOrDefault();
        PriorityComboBox.SelectedItem = priorityVM;
    }

    private void CancelBtn_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void OkBtn_Click(object sender, RoutedEventArgs e)
    {
        var chosenPriorityVM = (PriorityViewModel)PriorityComboBox.SelectedItem;
        ChosenPriority = chosenPriorityVM.Priority;
        DialogResult = true;
    }

    private class PriorityViewModel
    {
        public ProcessPriorityClass Priority { get; set; }
        public string PriorityString { get; set; }

        public PriorityViewModel(string priorityString, ProcessPriorityClass priorityClass)
        {
            PriorityString = priorityString;
            Priority = priorityClass;
        }
    }

}
