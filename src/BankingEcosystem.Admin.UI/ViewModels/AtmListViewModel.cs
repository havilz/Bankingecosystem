using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using BankingEcosystem.Admin.UI.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Admin.UI.ViewModels;

public partial class AtmListViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;
    private readonly Action _navigateToAdd;
    private readonly Action<int, Action> _showRefill;

    [ObservableProperty]
    private ObservableCollection<AtmDto> _atms = [];

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _hasError;

    public AtmListViewModel(AdminApiService apiService, Action navigateToAdd, Action<int, Action> showRefill)
    {
        _apiService = apiService;
        _navigateToAdd = navigateToAdd;
        _showRefill = showRefill;
        LoadAtmsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadAtmsAsync()
    {
        IsLoading = true;
        HasError = false;
        ErrorMessage = null;
        Atms.Clear();

        try
        {
            var items = await _apiService.GetAtmsAsync();
            foreach (var item in items) Atms.Add(item);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = $"Failed to load ATMs: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void NavigateToAdd()
    {
        _navigateToAdd();
    }

    [RelayCommand]
    private async Task ToggleStatusAsync(int atmId)
    {
        try
        {
            await _apiService.ToggleAtmStatusAsync(atmId);
            await LoadAtmsAsync(); // Refresh list
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Toggle Status Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void ShowRefill(int atmId)
    {
        _showRefill(atmId, async () => await LoadAtmsAsync());
    }
}
