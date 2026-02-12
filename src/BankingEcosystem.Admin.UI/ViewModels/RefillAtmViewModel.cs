using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using BankingEcosystem.Admin.UI.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Admin.UI.ViewModels;

public partial class RefillAtmViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;
    private readonly Action _onClose;

    public int AtmId { get; private set; }
    private Func<Task>? _onRefillComplete;

    [ObservableProperty]
    private string _targetAtmInfo = string.Empty;

    [ObservableProperty]
    private int _selectedDenomination = 50000;

    [ObservableProperty]
    private int _quantity = 100;

    [ObservableProperty]
    private bool _isBusy;

    public ObservableCollection<int> Denominations { get; } = [50000, 100000];

    public RefillAtmViewModel(AdminApiService apiService, Action onClose)
    {
        _apiService = apiService;
        _onClose = onClose;
    }

    public void Initialize(int atmId, Func<Task> onRefillComplete)
    {
        AtmId = atmId;
        _onRefillComplete = onRefillComplete;
        TargetAtmInfo = $"Refilling ATM ID: {atmId}";
    }

    [RelayCommand]
    private async Task SubmitRefillAsync()
    {
        if (Quantity <= 0)
        {
            MessageBox.Show("Quantity must be greater than 0.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        IsBusy = true;
        try
        {
            await _apiService.RefillAtmAsync(new RefillAtmRequest(AtmId, SelectedDenomination, Quantity));
            MessageBox.Show("ATM Refill Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            
            if (_onRefillComplete != null)
                await _onRefillComplete();
                
            _onClose();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Refill Failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _onClose();
    }
}
