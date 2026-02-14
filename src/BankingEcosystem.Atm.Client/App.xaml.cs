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
        
        // Register HttpClient with Polly Retry Policy
        services.AddHttpClient("", client => 
        {
            client.BaseAddress = new Uri("http://localhost:5046/");
        })
        .AddStandardResilienceHandler(); // Uses default exponential backoff

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

        // Change PIN Feature
        services.AddTransient<ViewModels.ChangePinViewModel>();
        services.AddTransient<Views.ChangePinView>();

        // Deposit Feature
        services.AddTransient<ViewModels.DepositViewModel>();
        services.AddTransient<Views.DepositView>();

        // Withdraw Feature
        services.AddTransient<ViewModels.WithdrawViewModel>();
        services.AddTransient<Views.WithdrawView>();
        
        // Error Feature
        services.AddTransient<ViewModels.ErrorViewModel>();
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
        string errorMessage = $"Critial Error: {e.Exception.Message}";
        if (e.Exception.InnerException != null)
        {
            errorMessage += $"\n{e.Exception.InnerException.Message}";
        }
        
        Log("CRITICAL ERROR: " + errorMessage + "\n" + e.Exception.StackTrace);

        // Show friendly Error View
        Application.Current.Dispatcher.Invoke(() =>
        {
            var errorView = new Views.ErrorView();
            var vm = new ViewModels.ErrorViewModel();
            vm.ErrorMessage = errorMessage; // Set specific message
            errorView.DataContext = vm;
            errorView.ShowDialog();
        });

        e.Handled = true;
        Application.Current.Shutdown();
    }

    public static void Log(string message)
    {
        try
        {
            System.IO.File.AppendAllText("client_errors.log", $"{DateTime.Now}: {message}\n");
        }
        catch { }
    }
}

