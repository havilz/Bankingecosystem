using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Admin.UI.Services;

namespace BankingEcosystem.Admin.UI.ViewModels;

public partial class CustomerListViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;
    private readonly Action<int> _navigateToDetail;
    private readonly Action _navigateToAdd;
    private List<CustomerDto> _allCustomers = [];

    [ObservableProperty]
    private ObservableCollection<CustomerDto> _customers = [];

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    public CustomerListViewModel(AdminApiService apiService, Action<int> navigateToDetail, Action navigateToAdd)
    {
        _apiService = apiService;
        _navigateToDetail = navigateToDetail;
        _navigateToAdd = navigateToAdd;
        _ = LoadCustomersAsync();
    }

    [RelayCommand]
    private async Task LoadCustomersAsync()
    {
        IsLoading = true;
        try
        {
            _allCustomers = await _apiService.GetCustomersAsync();
            ApplyFilter();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Gagal memuat data nasabah:\n{ex.Message}", "Error",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Search()
    {
        ApplyFilter();
    }

    [RelayCommand]
    private void ViewDetail(CustomerDto customer)
    {
        _navigateToDetail(customer.CustomerId);
    }

    [RelayCommand]
    private void AddCustomer()
    {
        _navigateToAdd();
    }

    private void ApplyFilter()
    {
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allCustomers
            : _allCustomers.Where(c =>
                c.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                c.NIK.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                (c.Email ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        Customers = new ObservableCollection<CustomerDto>(filtered);
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }
}
