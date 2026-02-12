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
    private readonly AdminApiService _apiService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ObservableObject? _currentView;

    [ObservableProperty]
    private bool _isLoggedIn;

    [ObservableProperty]
    private string _employeeName = string.Empty;

    public MainViewModel(NavigationService navigationService, AdminAuthService authService, AdminApiService apiService, IServiceProvider serviceProvider)
    {
        _navigationService = navigationService;
        _authService = authService;
        _apiService = apiService;
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
    private void NavigateToCustomers()
    {
        var vm = new CustomerListViewModel(
            _apiService,
            navigateToDetail: id => NavigateToCustomerDetail(id),
            navigateToAdd: () => NavigateToAddCustomer()
        );
        CurrentView = vm;
    }

    private void NavigateToCustomerDetail(int customerId)
    {
        var vm = new CustomerDetailViewModel(
            _apiService,
            navigateBack: () => NavigateToCustomers()
        );
        CurrentView = vm;
        _ = vm.LoadAsync(customerId);
    }

    private void NavigateToAddCustomer()
    {
        var vm = new AddCustomerViewModel(
            _apiService,
            onSaved: () => NavigateToCustomers(),
            onCancel: () => NavigateToCustomers()
        );
        CurrentView = vm;
    }

    [RelayCommand]
    private void NavigateToAtms()
    {
        var vm = new AtmListViewModel(
            _apiService,
            navigateToAdd: () => NavigateToAddAtm(),
            showRefill: (id, onRefillDone) => ShowRefillAtm(id, onRefillDone)
        );
        CurrentView = vm;
    }

    private void NavigateToAddAtm()
    {
        var vm = new AddAtmViewModel(
            _apiService,
            onSuccess: () => NavigateToAtms(),
            onCancel: () => NavigateToAtms()
        );
        CurrentView = vm;
    }

    private void ShowRefillAtm(int atmId, Action onRefillDone)
    {
        var vm = new RefillAtmViewModel(
            _apiService,
            onClose: () => 
            {
                NavigateToAtms();
                // We might want to refresh the list, but NavigateToAtms creates a new VM which loads. 
                // So onRefillDone is implicitly handled by reloading the list in the new VM.
                // However, the original callback required "Action onRefillDone".
                // We can just NavigateToAtms() which is simpler.
            }
        );
        vm.Initialize(atmId, async () => { await Task.Yield(); }); // No extra action needed as we navigate back
        CurrentView = vm;
    }

    [RelayCommand]
    private void Logout()
    {
        _authService.Logout();
        NavigateToLogin();
    }
}

