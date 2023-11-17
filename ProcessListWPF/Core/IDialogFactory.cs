using System.Windows;

namespace ProcessListWPF.Core;

public interface IDialogFactory<TWindow> where TWindow : Window
{
    TWindow Create();
}
