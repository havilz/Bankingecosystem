using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace BankingEcosystem.Atm.UI.Views;

public partial class MainMenuView : Page
{
    private readonly DispatcherTimer _clockTimer;

    public MainMenuView()
    {
        InitializeComponent();

        _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clockTimer.Tick += (_, _) => UpdateClock();
        _clockTimer.Start();
        UpdateClock();

        // TODO: Set customer name and balance from session/API data
        CustomerNameText.Text = "Nasabah Bank Ecosystem";
        BalanceText.Text = "Rp ********";

        Loaded += (_, _) => Focus();
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
    }

    // --- Menu Click Handlers ---

    private void OnWithdrawClick(object sender, RoutedEventArgs e)
    {
        // TODO: Navigate to WithdrawalView
        ShowComingSoon("Tarik Tunai");
    }

    private void OnDepositClick(object sender, RoutedEventArgs e)
    {
        // TODO: Navigate to DepositView
        ShowComingSoon("Setor Tunai");
    }

    private void OnTransferClick(object sender, RoutedEventArgs e)
    {
        // TODO: Navigate to TransferView
        ShowComingSoon("Transfer");
    }

    private void OnBalanceClick(object sender, RoutedEventArgs e)
    {
        // TODO: Navigate to BalanceInquiryView
        ShowComingSoon("Cek Saldo");
    }

    private void OnHistoryClick(object sender, RoutedEventArgs e)
    {
        // TODO: Navigate to HistoryView
        ShowComingSoon("Riwayat Transaksi");
    }

    private void OnChangePinClick(object sender, RoutedEventArgs e)
    {
        // TODO: Navigate to ChangePinView
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
