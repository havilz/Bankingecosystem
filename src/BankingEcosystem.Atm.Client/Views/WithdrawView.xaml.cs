using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BankingEcosystem.Atm.UI.ViewModels;

namespace BankingEcosystem.Atm.UI.Views;

public partial class WithdrawView : Page
{
    private readonly WithdrawViewModel _viewModel;

    public WithdrawView(WithdrawViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        
        _viewModel.RequestClose += OnRequestClose;
        Loaded += (_, _) => Focus();
    }

    private void OnRequestClose()
    {
        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.NavigateTo<MainMenuView>();
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (_viewModel.IsBusy) return;

        // Number Keys
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
        {
            _viewModel.DigitCommand.Execute((e.Key - Key.D0).ToString());
        }
        else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        {
             _viewModel.DigitCommand.Execute((e.Key - Key.NumPad0).ToString());
        }
        // Actions
        else if (e.Key == Key.Back)
        {
            _viewModel.BackspaceCommand.Execute(null);
        }
        else if (e.Key == Key.Enter)
        {
            _viewModel.EnterCommand.Execute(null);
        }
        else if (e.Key == Key.Escape)
        {
            _viewModel.CancelCommand.Execute(null);
        }
    }
}
