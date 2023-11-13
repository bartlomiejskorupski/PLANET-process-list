using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProcessListWPF.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        MenuContentControl.Content = App.AppHost!.Services.GetRequiredService<MenuView>();
    }

    private void ListViewColumnHeaderClick(object sender, RoutedEventArgs e)
    {
        GridViewColumnHeader column = (GridViewColumnHeader)sender;
        string sortBy = column.Tag.ToString()!;

        ICollectionView view = CollectionViewSource.GetDefaultView(ProcessListView.ItemsSource);
        ListSortDirection newDirection = ListSortDirection.Ascending;

        if (view.SortDescriptions.Count > 0 && view.SortDescriptions[0].PropertyName == sortBy)
        {
            newDirection = (view.SortDescriptions[0].Direction == ListSortDirection.Ascending) ?
                ListSortDirection.Descending : ListSortDirection.Ascending;
        }

        view.SortDescriptions.Clear();
        view.SortDescriptions.Add(new SortDescription(sortBy, newDirection));
    }

}
