using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BankingEcosystem.Atm.AppLayer.Services;

namespace BankingEcosystem.Atm.UI.Views;

public partial class OnboardingView : Page
{
    private readonly DispatcherTimer _clockTimer;
    private readonly IAuthService _authService;
    private readonly AtmSessionService _sessionService;

    // Design-time fallback
    public OnboardingView() : this(null!, null!) { }

    [Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
    public OnboardingView(IAuthService authService, AtmSessionService sessionService)
    {
        _authService = authService;
        _sessionService = sessionService;

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
                _ = InsertCardAsync();
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
        _ = InsertCardAsync();
    }

    private void OnCardlessClick(object sender, RoutedEventArgs e)
    {
        NavigateToCardless();
    }

    private async Task InsertCardAsync()
    {
        // Simulate reading card number from hardware
        // In production, this would come from HardwareInteropService.ReadCard()
        string simulatedCardNumber = "6221453754749809"; // Test card seeded in DB

        if (_authService == null)
        {
            // Fallback: no DI, just navigate
            NavigateToPinEntry();
            return;
        }

        // Step 1: Verify card with Backend API
        var cardResult = await _authService.VerifyCardAsync(simulatedCardNumber);

        if (cardResult == null)
        {
            MessageBox.Show(
                "Kartu tidak ditemukan. Pastikan kartu Anda terdaftar.",
                "Kartu Tidak Valid",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (cardResult.IsBlocked)
        {
            MessageBox.Show(
                "Kartu Anda telah diblokir. Silakan hubungi bank.",
                "Kartu Diblokir",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        // Step 2: Store card info in session
        _sessionService.StartSession(simulatedCardNumber);
        _sessionService.CardId = cardResult.CardId;

        // Step 3: Navigate to PIN entry
        NavigateToPinEntry();
    }

    private void NavigateToPinEntry()
    {
        _clockTimer.Stop();

        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.NavigateTo<PinEntryView>();
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
