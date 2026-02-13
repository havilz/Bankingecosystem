using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BankingEcosystem.Atm.UI.Views;

public partial class PinEntryView : Page
{
    private const int MaxPinLength = 6;
    private string _currentPin = string.Empty;
    private readonly Ellipse[] _dots;

    private readonly System.Windows.Threading.DispatcherTimer _timer;

    private readonly BankingEcosystem.Atm.AppLayer.Services.IAuthService _authService;
    private readonly BankingEcosystem.Atm.AppLayer.Services.AtmSessionService _sessionService;
    private readonly BankingEcosystem.Atm.AppLayer.Services.IAtmStateService _atmStateService;

    // Default constructor for design-time support or fallback
    public PinEntryView() : this(null!, null!, null!) { }

    [Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
    public PinEntryView(BankingEcosystem.Atm.AppLayer.Services.IAuthService authService, 
                        BankingEcosystem.Atm.AppLayer.Services.AtmSessionService sessionService,
                        BankingEcosystem.Atm.AppLayer.Services.IAtmStateService atmStateService)
    {
        _authService = authService;
        _sessionService = sessionService;
        _atmStateService = atmStateService;

        InitializeComponent();

        _dots = [Dot1, Dot2, Dot3, Dot4, Dot5, Dot6];
        StatusText.Text = $"Masukkan {MaxPinLength} digit PIN";

        _timer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += (s, e) => UpdateClock();

        Loaded += (_, _) => 
        {
            Focus();
            UpdateClock();
            _timer.Start();
            
            // FSM: Transition to PIN Entry
            try
            {
                 // If we are coming from Idle (dev mode) or CardInserted (normal), this should work.
                 // If C++ logic is strict, it requires CardInserted first.
                 // We assume previous step set it to CardInserted.
                _atmStateService.TransitionTo(BankingEcosystem.Atm.AppLayer.Models.AtmState.PinEntry);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FSM Error: {ex.Message}");
            }
        };

        Unloaded += (_, _) => _timer.Stop();
        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Toggle layout based on width (approx 1000px threshold)
        // Large Screen: Keypad Visible, content left-aligned with custom margins.
        // Small Screen: Keypad Hidden, content centered with 0 margins.
        bool isLargeScreen = ActualWidth > 1000;
        
        if (KeypadGrid != null)
        {
            KeypadGrid.Visibility = isLargeScreen ? Visibility.Visible : Visibility.Collapsed;
            
            if (InfoPanel != null)
            {
                // Toggle Panel Margin
                InfoPanel.Margin = isLargeScreen ? new Thickness(0, 0, 64, 0) : new Thickness(0);

                // Toggle Alignment & Margins for inner elements
                var alignment = isLargeScreen ? HorizontalAlignment.Left : HorizontalAlignment.Center;
                
                if (LockBorder != null) LockBorder.HorizontalAlignment = alignment;
                
                if (TitleText != null)
                {
                    TitleText.HorizontalAlignment = alignment;
                    TitleText.Margin = isLargeScreen ? new Thickness(-76, 0, 0, 8) : new Thickness(0, 0, 0, 8);
                }

                if (SubtitleText != null)
                {
                    SubtitleText.HorizontalAlignment = alignment;
                    SubtitleText.Margin = isLargeScreen ? new Thickness(-76, 0, 0, 32) : new Thickness(0, 0, 0, 32);
                }

                if (PinDotsPanel != null)
                {
                    PinDotsPanel.HorizontalAlignment = alignment;
                    PinDotsPanel.Margin = isLargeScreen ? new Thickness(-160, 0, 0, 12) : new Thickness(0, 0, 0, 12);
                }

                if (StatusText != null)
                {
                    StatusText.HorizontalAlignment = alignment;
                    StatusText.Margin = isLargeScreen ? new Thickness(-56, 0, 0, 0) : new Thickness(0);
                }
            }
        }
    }

    private void UpdateClock()
    {
        ClockText.Text = DateTime.Now.ToString("HH:mm");
    }

    // --- Keyboard input ---

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
        {
            AddDigit((char)('0' + (e.Key - Key.D0)));
            e.Handled = true;
        }
        else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        {
            AddDigit((char)('0' + (e.Key - Key.NumPad0)));
            e.Handled = true;
        }
        else if (e.Key == Key.Back)
        {
            RemoveLastDigit();
            e.Handled = true;
        }
        else if (e.Key == Key.Delete)
        {
            ClearPin();
            e.Handled = true;
        }
        else if (e.Key == Key.Enter && _currentPin.Length == MaxPinLength)
        {
            SubmitPin();
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            GoBack();
            e.Handled = true;
        }
    }

    // --- On-screen keypad ---

    private void OnKeypadClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Content is string digit)
        {
            AddDigit(digit[0]);
        }
        Focus(); // Keep focus for keyboard
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        GoBack();
    }

    private void OnConfirmClick(object sender, RoutedEventArgs e)
    {
        SubmitPin();
    }

    // --- PIN logic ---

    private void AddDigit(char digit)
    {
        if (_currentPin.Length >= MaxPinLength) return;

        _currentPin += digit;
        UpdateDots();
    }

    private void RemoveLastDigit()
    {
        if (_currentPin.Length == 0) return;

        _currentPin = _currentPin[..^1];
        UpdateDots();
    }

    private void ClearPin()
    {
        _currentPin = string.Empty;
        UpdateDots();
    }

    private void UpdateDots()
    {
        var goldBrush = (SolidColorBrush)FindResource("GoldBrush");
        var emptyBrush = new SolidColorBrush(Color.FromArgb(0x30, 0xFF, 0xFF, 0xFF));

        for (int i = 0; i < MaxPinLength; i++)
        {
            _dots[i].Fill = i < _currentPin.Length ? goldBrush : emptyBrush;
        }

        if (_currentPin.Length == MaxPinLength)
            StatusText.Text = "Tekan Enter untuk melanjutkan";
        else if (_currentPin.Length > 0)
            StatusText.Text = $"{MaxPinLength - _currentPin.Length} digit lagi";
        else
            StatusText.Text = $"Masukkan {MaxPinLength} digit PIN";
    }

    private async void SubmitPin()
    {
        if (_currentPin.Length != MaxPinLength) return;

        StatusText.Text = "Memverifikasi PIN...";
        StatusText.Foreground = new SolidColorBrush(Color.FromRgb(255, 211, 116)); // Gold

        bool isValid = false;
        
        if (_authService != null && _sessionService?.CardId != null)
        {
            // Real flow: use CardId from session (set during VerifyCard in OnboardingView)
            isValid = await _authService.VerifyPinAsync(_sessionService.CardId.Value, _currentPin);
        }
        else
        {
            // Fallback for testing without backend
            await Task.Delay(500);
            isValid = _currentPin == "123456"; 
        }

        if (isValid)
        {
            StatusText.Text = "PIN Diterima ✓";
            StatusText.Foreground = new SolidColorBrush(Colors.LightGreen);
            
            // FSM: Transition to Authenticated
            try { _atmStateService.TransitionTo(BankingEcosystem.Atm.AppLayer.Models.AtmState.Authenticated); } catch {}

            await Task.Delay(500); 

            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateTo<MainMenuView>();
            }
        }
        else
        {
            StatusText.Text = "PIN Salah! Silakan coba lagi.";
            StatusText.Foreground = new SolidColorBrush(Colors.Red);
            ClearPin();
        }
    }

    private void GoBack()
    {
        // FSM: Return to Idle (Eject Card)
        try { _atmStateService.TransitionTo(BankingEcosystem.Atm.AppLayer.Models.AtmState.Idle); } catch {}

        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.NavigateToOnboarding();
        }
    }
}
