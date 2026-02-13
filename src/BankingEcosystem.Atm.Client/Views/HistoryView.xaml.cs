using System.Windows;
using System.Windows.Controls;
using BankingEcosystem.Atm.UI.ViewModels;

namespace BankingEcosystem.Atm.UI.Views;

public partial class HistoryView : Page
{
    private readonly HistoryViewModel? _viewModel;

    // Design-time fallback
    public HistoryView() : this(null!) { }

    [Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
    public HistoryView(HistoryViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;

        InitializeComponent();

        if (_viewModel != null)
        {
            _viewModel.RequestClose += OnRequestClose;
        }

        Loaded += async (_, _) => 
        {
            if (_viewModel != null)
            {
                await _viewModel.LoadHistoryAsync();
            }
        };

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
}
