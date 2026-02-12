using System.Windows.Controls;
using BankingEcosystem.Admin.UI.ViewModels;

namespace BankingEcosystem.Admin.UI.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
        {
            vm.SetPassword(PasswordBox.Password);
        }
    }
}
