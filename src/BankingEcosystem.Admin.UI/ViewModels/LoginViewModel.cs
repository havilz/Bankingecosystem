using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Admin.UI.Services;

namespace BankingEcosystem.Admin.UI.ViewModels;

/// <summary>
/// ViewModel for the Employee Login screen.
/// </summary>
public partial class LoginViewModel : ObservableObject
{
    private readonly AdminAuthService _authService;

    /// <summary>Callback invoked when login succeeds (set by MainViewModel).</summary>
    public Action? LoginSucceeded { get; set; }

    [ObservableProperty]
    private string _employeeCode = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public LoginViewModel(AdminAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Called from the View's code-behind to set the password (PasswordBox can't bind directly).
    /// </summary>
    public void SetPassword(string password)
    {
        Password = password;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(EmployeeCode))
        {
            ErrorMessage = "Employee Code harus diisi.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Password harus diisi.";
            return;
        }

        IsBusy = true;

        try
        {
            var error = await _authService.LoginAsync(EmployeeCode.Trim(), Password);

            if (error == null)
            {
                // Login succeeded
                LoginSucceeded?.Invoke();
            }
            else
            {
                ErrorMessage = error;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
