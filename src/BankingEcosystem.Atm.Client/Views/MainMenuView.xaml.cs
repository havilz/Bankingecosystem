using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BankingEcosystem.Atm.AppLayer.Services;

namespace BankingEcosystem.Atm.UI.Views;

public partial class MainMenuView : Page
{
    private readonly DispatcherTimer _clockTimer;
    private readonly AtmSessionService _sessionService;
    private readonly ITransactionService _transactionService;

    // Design-time fallback
    public MainMenuView() : this(null!, null!) { }

    [Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
    public MainMenuView(AtmSessionService sessionService, ITransactionService transactionService)
    {
        _sessionService = sessionService;
        _transactionService = transactionService;

        InitializeComponent();

        _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clockTimer.Tick += (_, _) => UpdateClock();
        _clockTimer.Start();
        UpdateClock();

        // Display real session data
        CustomerNameText.Text = _sessionService?.CustomerName ?? "Nasabah";
        BalanceText.Text = "Rp ********"; // Masked by default until Balance Inquiry

        Loaded += (_, _) => Focus();

        // Listen for session expiry
        if (_sessionService != null)
        {
            _sessionService.SessionExpired += OnSessionExpired;
        }
    }

    private void OnSessionExpired(object? sender, EventArgs e)
    {
        // Session expired — navigate back to Onboarding on UI thread
        Dispatcher.Invoke(() =>
        {
            _clockTimer.Stop();
            MessageBox.Show(
                "Sesi Anda telah berakhir karena tidak ada aktivitas.",
                "Sesi Berakhir",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToOnboarding();
            }
        });
    }

    private void UpdateClock()
    {
        ClockText.Text = DateTime.Now.ToString("HH:mm:ss");
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            ExitToOnboarding();
            e.Handled = true;
        }

        // Refresh session timer on any key press
        _sessionService?.RefreshSession();
    }

    // --- Menu Click Handlers ---

    private void OnWithdrawClick(object sender, RoutedEventArgs e)
    {
        _sessionService?.RefreshSession();
        // TODO: Navigate to WithdrawalView (Phase 3)
        ShowComingSoon("Tarik Tunai");
    }

    private void OnDepositClick(object sender, RoutedEventArgs e)
    {
        _sessionService?.RefreshSession();
        ShowComingSoon("Setor Tunai");
    }

    private void OnTransferClick(object sender, RoutedEventArgs e)
    {
        _sessionService?.RefreshSession();
        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.NavigateTo<TransferView>();
        }
    }

    private async void OnBalanceClick(object sender, RoutedEventArgs e)
    {
        _sessionService?.RefreshSession();

        if (_transactionService == null)
        {
            ShowComingSoon("Cek Saldo");
            return;
        }

        BalanceText.Text = "Memuat...";
        var balance = await _transactionService.GetBalanceAsync();
        
        if (balance.HasValue)
        {
            BalanceText.Text = $"Rp {balance.Value:N0}";
        }
        else
        {
            BalanceText.Text = "Gagal memuat saldo";
            MessageBox.Show(
                "Tidak dapat mengambil data saldo. Periksa koneksi ke server.",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private void OnHistoryClick(object sender, RoutedEventArgs e)
    {
        _sessionService?.RefreshSession();
        ShowComingSoon("Riwayat Transaksi");
    }

    private void OnChangePinClick(object sender, RoutedEventArgs e)
    {
        _sessionService?.RefreshSession();
        ShowComingSoon("Ubah PIN");
    }

    private void OnExitClick(object sender, RoutedEventArgs e)
    {
        ExitToOnboarding();
    }

    // --- Helpers ---

    private void ExitToOnboarding()
    {
        _clockTimer.Stop();
        _sessionService?.EndSession();

        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.NavigateToOnboarding();
        }
    }

    private static void ShowComingSoon(string featureName)
    {
        MessageBox.Show(
            $"Fitur \"{featureName}\" akan diimplementasikan di fase berikutnya.",
            featureName,
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}
