﻿using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;


namespace ProcessListWPF.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        MenuContentControl.Content = App.AppHost!.Services.GetRequiredService<MenuView>();
    }

}
