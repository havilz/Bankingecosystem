using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Admin.UI.Services;

namespace BankingEcosystem.Admin.UI.ViewModels;

/// <summary>
/// Shell ViewModel — manages navigation, login state, and the top-level nav bar.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;
    private readonly AdminAuthService _authService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ObservableObject? _currentView;

    [ObservableProperty]
    private bool _isLoggedIn;

    [ObservableProperty]
    private string _employeeName = string.Empty;

    public MainViewModel(NavigationService navigationService, AdminAuthService authService, IServiceProvider serviceProvider)
    {
        _navigationService = navigationService;
        _authService = authService;
        _serviceProvider = serviceProvider;

        // Start at Login
        NavigateToLogin();
    }

    public void NavigateToLogin()
    {
        var loginVm = (LoginViewModel)_serviceProvider.GetService(typeof(LoginViewModel))!;
        loginVm.LoginSucceeded = OnLoginSucceeded;
        CurrentView = loginVm;
        IsLoggedIn = false;
        EmployeeName = string.Empty;
    }

    private void OnLoginSucceeded()
    {
        IsLoggedIn = true;
        EmployeeName = _authService.EmployeeName ?? "Employee";
        NavigateToDashboard();
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        var dashboardVm = (DashboardViewModel)_serviceProvider.GetService(typeof(DashboardViewModel))!;
        dashboardVm.EmployeeName = _authService.EmployeeName ?? "Employee";
        dashboardVm.EmployeeRole = _authService.EmployeeRole ?? "Staff";
        CurrentView = dashboardVm;
    }

    [RelayCommand]
    private void Logout()
    {
        _authService.Logout();
        NavigateToLogin();
    }
}
