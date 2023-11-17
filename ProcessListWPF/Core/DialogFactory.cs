using System;
using System.Windows;

namespace ProcessListWPF.Core;

public class DialogFactory<TWindow>: IDialogFactory<TWindow> where TWindow : Window
{
    private readonly Func<TWindow> _factoryFunc;

    public DialogFactory(Func<TWindow> factoryFunc)
    {
        _factoryFunc = factoryFunc;
    }

    public TWindow Create()
    {
        return _factoryFunc();
    }
}
