﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProcessListWPF.Core;
using ProcessListWPF.Services;
using ProcessListWPF.ViewModels;
using ProcessListWPF.ViewModels.Home;
using ProcessListWPF.Views;
using ProcessListWPF.Views.Shared;
using System;
using System.Windows;

namespace ProcessListWPF;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices(ConfigureApplicationServices)
            .Build();
    }

    private void ConfigureApplicationServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddSingleton<IRefreshService, RefreshService>();

        services.AddTransient<ChangePriorityWindow>();
        services.AddSingleton<Func<ChangePriorityWindow>>(s => () => s.GetRequiredService<ChangePriorityWindow>());
        services.AddSingleton<IDialogFactory<ChangePriorityWindow>, DialogFactory<ChangePriorityWindow>>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<MenuViewModel>();
        services.AddTransient<DetailsViewModel>();

        services.AddSingleton(s => new MainWindow()
        {
            DataContext = s.GetRequiredService<MainViewModel>()
        });
        services.AddSingleton(s => new MenuView()
        {
            DataContext = s.GetRequiredService<MenuViewModel>()
        });
        services.AddSingleton<HomeView>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }

}
