using System.Windows;
using System.Windows.Controls;
using BankingEcosystem.Atm.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BankingEcosystem.Atm.UI;

public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;

    public MainWindow(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        App.Log("MainWindow Constructor started");
        InitializeComponent();
        App.Log("MainWindow InitializeComponent finished");

        Loaded += (_, _) => App.Log("MainWindow Loaded");
        
        // Initial navigation
        NavigateTo<OnboardingView>();
        App.Log("MainWindow Initial Navigation done");
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
