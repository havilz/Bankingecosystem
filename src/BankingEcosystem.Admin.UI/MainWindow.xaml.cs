using System.Windows;
using BankingEcosystem.Admin.UI.ViewModels;

namespace BankingEcosystem.Admin.UI;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}