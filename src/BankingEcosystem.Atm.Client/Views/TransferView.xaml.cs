using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BankingEcosystem.Atm.UI.ViewModels;

namespace BankingEcosystem.Atm.UI.Views;

public partial class TransferView : Page
{
    private readonly TransferViewModel? _viewModel;

    // Design-time fallback
    public TransferView() : this(null!) { }

    [Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
    public TransferView(TransferViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;

        InitializeComponent();

        if (_viewModel != null)
        {
            _viewModel.RequestClose += OnRequestClose;
        }

        Loaded += (_, _) => Focus();
        Unloaded += (_, _) => 
        {
            if (_viewModel != null) _viewModel.RequestClose -= OnRequestClose;
        };
    }

    private void OnRequestClose()
    {
        if (Window.GetWindow(this) is MainWindow mw)
        {
            mw.NavigateTo<MainMenuView>();
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (_viewModel == null) return;

        if (e.Key >= Key.D0 && e.Key <= Key.D9)
        {
            _viewModel.DigitCommand.Execute((e.Key - Key.D0).ToString());
            e.Handled = true;
        }
        else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        {
            _viewModel.DigitCommand.Execute((e.Key - Key.NumPad0).ToString());
            e.Handled = true;
        }
        else if (e.Key == Key.Back)
        {
            _viewModel.BackspaceCommand.Execute(null);
            e.Handled = true;
        }
        else if (e.Key == Key.Delete)
        {
            _viewModel.ClearCommand.Execute(null);
            e.Handled = true;
        }
        else if (e.Key == Key.Enter)
        {
            _viewModel.EnterCommand.Execute(null);
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            _viewModel.CancelCommand.Execute(null);
            e.Handled = true;
        }
    }
}
