using System.Windows.Controls;
using BankingEcosystem.Admin.UI.ViewModels;

namespace BankingEcosystem.Admin.UI.Views;

public partial class CustomerDetailView : UserControl
{
    public CustomerDetailView() => InitializeComponent();

    private void OnPinChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is CustomerDetailViewModel vm && sender is PasswordBox pb)
        {
            vm.NewCardPin = pb.Password;
        }
    }
}
