using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace BankingEcosystem.Atm.UI.Views;

public partial class OnboardingView : Page
{
    private readonly DispatcherTimer _clockTimer;

    public OnboardingView()
    {
        InitializeComponent();

        // Clock timer — update every second
        _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clockTimer.Tick += (_, _) => UpdateClock();
        _clockTimer.Start();
        UpdateClock();

        // Focus for keyboard input
        Loaded += (_, _) => Focus();
    }

    private void UpdateClock()
    {
        ClockText.Text = DateTime.Now.ToString("HH:mm:ss");
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.C:
                NavigateToPinEntry();
                e.Handled = true;
                break;

            case Key.Enter:
                NavigateToCardless();
                e.Handled = true;
                break;
        }
    }

    private void OnCardInsertClick(object sender, RoutedEventArgs e)
    {
        NavigateToPinEntry();
    }

    private void OnCardlessClick(object sender, RoutedEventArgs e)
    {
        NavigateToCardless();
    }

    private void NavigateToPinEntry()
    {
        _clockTimer.Stop();

        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.NavigateTo(new PinEntryView());
        }
    }

    private void NavigateToCardless()
    {
        // Placeholder — will be implemented when mbank features are designed
        MessageBox.Show(
            "Fitur Transaksi Tanpa Kartu akan segera hadir.\nTunggu update selanjutnya!",
            "Coming Soon",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}
