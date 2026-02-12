using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Admin.UI.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Admin.UI.ViewModels;

/// <summary>
/// Placeholder Dashboard ViewModel — displays a welcome message and placeholder stats.
/// Real data fetching will be added in later steps.
/// </summary>
public partial class DashboardViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;

    [ObservableProperty]
    private string _employeeName = "Employee";

    [ObservableProperty]
    private string _employeeRole = "Staff";

    [ObservableProperty]
    private string _currentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy");

    [ObservableProperty]
    private DashboardStatsDto _stats;

    [ObservableProperty]
    private bool _isLoading;

    public DashboardViewModel(AdminApiService apiService)
    {
        _apiService = apiService;
        _stats = new DashboardStatsDto(0, 0, 0, 0);
        LoadStatsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadStatsAsync()
    {
        IsLoading = true;
        try
        {
            Stats = await _apiService.GetDashboardStatsAsync();
        }
        catch (Exception)
        {
            // Ignore for now, stats will remain 0
        }
        finally
        {
            IsLoading = false;
        }
    }
}
