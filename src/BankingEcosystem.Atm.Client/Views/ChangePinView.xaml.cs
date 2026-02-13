using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using BankingEcosystem.Atm.UI.ViewModels;

namespace BankingEcosystem.Atm.UI.Views;

public partial class ChangePinView : Page
{
    private readonly ChangePinViewModel _viewModel;
    private string _currentPin = "";

    public ChangePinView(ChangePinViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;

        Loaded += (_, _) => Focus();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (_viewModel.IsBusy) return;

        // Handle Numbers
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
        {
            AddDigit((e.Key - Key.D0).ToString());
        }
        else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        {
            AddDigit((e.Key - Key.NumPad0).ToString());
        }
        // Handle Backspace
        else if (e.Key == Key.Back)
        {
            RemoveDigit();
        }
        // Handle Enter
        else if (e.Key == Key.Enter)
        {
            SubmitPin();
        }
        // Handle Esc
        else if (e.Key == Key.Escape)
        {
             if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateTo<MainMenuView>();
            }
        }
    }

    private void AddDigit(string digit)
    {
        if (_currentPin.Length < 6)
        {
            _currentPin += digit;
            UpdatePinDots();
        }
    }

    private void RemoveDigit()
    {
        if (_currentPin.Length > 0)
        {
            _currentPin = _currentPin.Substring(0, _currentPin.Length - 1);
            UpdatePinDots();
        }
    }

    private void UpdatePinDots()
    {
        if (PinDotsPanel.Children.Count < 6) return;

        for (int i = 0; i < 6; i++)
        {
            if (PinDotsPanel.Children[i] is Ellipse dot)
            {
                if (i < _currentPin.Length)
                {
                    dot.Fill = (Brush)FindResource("GoldBrush");
                }
                else
                {
                    dot.Fill = new SolidColorBrush(Color.FromArgb(0x30, 0xFF, 0xFF, 0xFF));
                }
            }
        }
    }

    private async void SubmitPin()
    {
        if (_currentPin.Length == 6)
        {
            await _viewModel.ProcessPinAsync(_currentPin, ResetPin, CloseView);
        }
    }

    private void ResetPin()
    {
        _currentPin = "";
        UpdatePinDots();
    }

    private void CloseView()
    {
        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            MessageBox.Show("PIN berhasil diubah. Silakan login kembali.", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
            mainWindow.NavigateToOnboarding();
        }
    }
}
