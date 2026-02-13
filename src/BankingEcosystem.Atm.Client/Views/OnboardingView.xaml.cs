using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BankingEcosystem.Atm.AppLayer.Services;

namespace BankingEcosystem.Atm.UI.Views;

public partial class OnboardingView : UserControl
{
    private readonly IAuthService _authService;
    private readonly AtmSessionService _sessionService;
    private readonly IAtmStateService _atmStateService;
    private readonly DispatcherTimer _clockTimer;

    public OnboardingView(IAuthService authService, AtmSessionService sessionService, IAtmStateService atmStateService)
    {
        _authService = authService;
        _sessionService = sessionService;
        _atmStateService = atmStateService;

        InitializeComponent();

        // Ensure ATM is in Idle state
        _atmStateService.Reset();

        // Clock timer — update every second
        _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clockTimer.Tick += (_, _) => UpdateClock();
        _clockTimer.Start();
        UpdateClock();

        // Focus for keyboard input
        Loaded += (_, _) => { Keyboard.Focus(this); };
    }

    private void UpdateClock()
    {
        ClockText.Text = DateTime.Now.ToString("HH:mm:ss");
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        // Ignore global keys if overlay is visible
        if (CardEntryOverlay.Visibility == Visibility.Visible)
        {
            if (e.Key == Key.Escape) OnCancelCardEntry(sender, e);
            return;
        }

        switch (e.Key)
        {
            case Key.C:
                ShowCardEntry();
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
        ShowCardEntry();
    }

    private void ShowCardEntry()
    {
        CardEntryOverlay.Visibility = Visibility.Visible;
        CardNumberInput.Text = string.Empty;
        CardNumberInput.Focus();
    }

    private void OnCancelCardEntry(object sender, RoutedEventArgs e)
    {
        CardEntryOverlay.Visibility = Visibility.Collapsed;
        Keyboard.Focus(this);
    }

    private void OnSubmitCardEntry(object sender, RoutedEventArgs e)
    {
        var cardNumber = CardNumberInput.Text.Trim();
        if (string.IsNullOrWhiteSpace(cardNumber)) return;

        CardEntryOverlay.Visibility = Visibility.Collapsed;
        _ = InsertCardAsync(cardNumber);
    }

    private void OnCardNumberKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            OnSubmitCardEntry(sender, e);
        }
    }

    private void OnCardlessClick(object sender, RoutedEventArgs e)
    {
        NavigateToCardless();
    }

    private async Task InsertCardAsync(string cardNumber)
    {
        // Step 1: Verify card with Backend API
        var cardResult = await _authService.VerifyCardAsync(cardNumber);

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
        _sessionService.StartSession(cardNumber);
        _sessionService.CardId = cardResult.CardId;

        // Step 2.5: C++ FSM Transition
        try
        {
            _atmStateService.TransitionTo(BankingEcosystem.Atm.AppLayer.Models.AtmState.CardInserted);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hardware Error: {ex.Message}", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

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
