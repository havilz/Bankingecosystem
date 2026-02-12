using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using BankingEcosystem.Admin.UI.Services;
using BankingEcosystem.Admin.UI.ViewModels;

namespace BankingEcosystem.Admin.UI;

public partial class App : Application
{
    public new static App Current => (App)Application.Current;
    public IServiceProvider Services { get; }

    public App()
    {
        Services = ConfigureServices();
        this.InitializeComponent();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // HTTP Client (named client for AdminApi)
        services.AddHttpClient("AdminApi", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5046/");
        });

        // Services
        services.AddSingleton<NavigationService>();
        services.AddSingleton<AdminAuthService>();
        services.AddSingleton<AdminApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new AdminApiService(factory.CreateClient("AdminApi"));
        });

        // ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<DashboardViewModel>();

        // Views
        services.AddSingleton<MainWindow>();

        return services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
