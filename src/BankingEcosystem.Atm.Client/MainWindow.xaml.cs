using System.Windows;
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
    /// Navigate to a specific page resolved from DI.
    /// </summary>
    public void NavigateTo<T>() where T : System.Windows.Controls.Page
    {
        var page = _serviceProvider.GetRequiredService<T>();
        MainFrame.Navigate(page);
    }

    /// <summary>
    /// Navigate to a specific page instance (legacy/manual).
    /// </summary>
    public void NavigateTo(System.Windows.Controls.Page page)
    {
        MainFrame.Navigate(page);
    }

    /// <summary>
    /// Helper for Onboarding navigation
    /// </summary>
    public void NavigateToOnboarding()
    {
        NavigateTo<OnboardingView>();
    }
}