using System.Windows;
using System.Windows.Controls;
using BankingEcosystem.Atm.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BankingEcosystem.Atm.UI;

public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BankingEcosystem.Atm.AppLayer.Services.IUiService _uiService;

    public MainWindow(IServiceProvider serviceProvider, BankingEcosystem.Atm.AppLayer.Services.IUiService uiService)
    {
        _serviceProvider = serviceProvider;
        _uiService = uiService;

        App.Log("MainWindow Constructor started");
        InitializeComponent();
        App.Log("MainWindow InitializeComponent finished");

        _uiService.IsBusyChanged += OnIsBusyChanged;

        Loaded += (_, _) => App.Log("MainWindow Loaded");
        
        // Initial navigation
        NavigateTo<OnboardingView>();
        App.Log("MainWindow Initial Navigation done");
    }

    private void OnIsBusyChanged(bool isBusy)
    {
        Dispatcher.Invoke(() => 
        {
            LoadingOverlay.Visibility = isBusy ? Visibility.Visible : Visibility.Collapsed;
        });
    }

    /// <summary>
    /// Navigate to a specific view resolved from DI.
    /// </summary>
    public void NavigateTo<T>() where T : FrameworkElement
    {
        var view = _serviceProvider.GetRequiredService<T>();
        MainFrame.Content = view;
    }

    /// <summary>
    /// Navigate to a specific view instance (legacy/manual).
    /// </summary>
    public void NavigateTo(FrameworkElement view)
    {
        MainFrame.Content = view;
    }

    /// <summary>
    /// Helper for Onboarding navigation
    /// </summary>
    public void NavigateToOnboarding()
    {
        NavigateTo<OnboardingView>();
    }
}
