using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using BankingEcosystem.Admin.UI.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Admin.UI.ViewModels;

public partial class AddAtmViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;
    private readonly Action _onSuccess;
    private readonly Action _onCancel;

    [ObservableProperty]
    private string _atmCode = string.Empty;

    [ObservableProperty]
    private string _location = string.Empty;

    [ObservableProperty]
    private decimal _initialCash = 0;

    [ObservableProperty]
    private bool _isBusy;

    public AddAtmViewModel(AdminApiService apiService, Action onSuccess, Action onCancel)
    {
        _apiService = apiService;
        _onSuccess = onSuccess;
        _onCancel = onCancel;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(AtmCode) || string.IsNullOrWhiteSpace(Location))
        {
            MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        IsBusy = true;
        try
        {
            await _apiService.CreateAtmAsync(new CreateAtmRequest(AtmCode, Location, InitialCash));
            MessageBox.Show("ATM created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _onSuccess();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to create ATM: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _onCancel();
    }
}
