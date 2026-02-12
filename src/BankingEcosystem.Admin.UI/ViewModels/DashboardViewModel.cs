using CommunityToolkit.Mvvm.ComponentModel;

namespace BankingEcosystem.Admin.UI.ViewModels;

/// <summary>
/// Placeholder Dashboard ViewModel — displays a welcome message and placeholder stats.
/// Real data fetching will be added in later steps.
/// </summary>
public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private string _employeeName = "Employee";

    [ObservableProperty]
    private string _employeeRole = "Staff";

    [ObservableProperty]
    private string _currentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy");
}
