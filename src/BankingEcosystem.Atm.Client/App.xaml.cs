using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Net.Http;

namespace BankingEcosystem.Atm.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        Log("App Constructor started");
        DispatcherUnhandledException += App_DispatcherUnhandledException;
        
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
    {
        // Core Services
        services.AddSingleton<BankingEcosystem.Atm.AppLayer.Services.AtmSessionService>();
        services.AddSingleton<BankingEcosystem.Atm.AppLayer.Services.IAtmStateService, BankingEcosystem.Atm.AppLayer.Services.AtmStateService>();
        
        // Register HttpClient
        services.AddSingleton(new HttpClient 
        { 
            BaseAddress = new Uri("http://localhost:5046/") 
        });

        // App Layer Services
        services.AddSingleton<BankingEcosystem.Atm.AppLayer.Services.IAuthService, BankingEcosystem.Atm.AppLayer.Services.AuthService>();
        services.AddSingleton<BankingEcosystem.Atm.AppLayer.Services.IHardwareInteropService, BankingEcosystem.Atm.AppLayer.Services.HardwareInteropService>();
        services.AddSingleton<BankingEcosystem.Atm.AppLayer.Services.ITransactionService, BankingEcosystem.Atm.AppLayer.Services.TransactionService>();

        // Views (Transient recommended for Pages)
        services.AddTransient<MainWindow>();
        services.AddTransient<Views.OnboardingView>();
        services.AddTransient<Views.PinEntryView>();
        services.AddTransient<Views.MainMenuView>();
        
        // Transfer Feature
        services.AddTransient<ViewModels.TransferViewModel>();
        services.AddTransient<Views.TransferView>();

        // History Feature
        services.AddTransient<ViewModels.HistoryViewModel>();
        services.AddTransient<Views.HistoryView>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        Log("App OnStartup started");
        base.OnStartup(e);

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        
        Log("App OnStartup finished");
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        string errorMessage = $"Unhandled Exception: {e.Exception.Message}";
        if (e.Exception.InnerException != null)
        {
            errorMessage += $"\nInner Exception: {e.Exception.InnerException.Message}";
        }
        
        Log("CRITICAL ERROR: " + errorMessage + "\n" + e.Exception.StackTrace);
        MessageBox.Show(errorMessage, "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }

    public static void Log(string message)
    {
        try
        {
            System.IO.File.AppendAllText("debug.log", $"{DateTime.Now}: {message}\n");
        }
        catch { }
    }
}

