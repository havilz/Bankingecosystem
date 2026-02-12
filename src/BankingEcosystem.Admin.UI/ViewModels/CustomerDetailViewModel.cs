using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Admin.UI.Services;

namespace BankingEcosystem.Admin.UI.ViewModels;

public partial class CustomerDetailViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;
    private readonly Action _navigateBack;

    [ObservableProperty] private string _customerName = string.Empty;
    [ObservableProperty] private string _nik = string.Empty;
    [ObservableProperty] private string _phone = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private string _dateOfBirth = string.Empty;
    [ObservableProperty] private string _createdAt = string.Empty;

    [ObservableProperty] private ObservableCollection<AccountDto> _accounts = [];
    [ObservableProperty] private ObservableCollection<CardDto> _cards = [];
    [ObservableProperty] private ObservableCollection<AccountTypeDto> _accountTypes = [];

    [ObservableProperty] private AccountTypeDto? _selectedAccountType;
    [ObservableProperty] private decimal _initialDeposit;
    [ObservableProperty] private string _newCardPin = string.Empty;
    [ObservableProperty] private AccountDto? _selectedAccountForCard;

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(HasStatusMessage))]
    private string _statusMessage = string.Empty;

    public bool HasStatusMessage => !string.IsNullOrEmpty(StatusMessage);

    private int _customerId;

    public CustomerDetailViewModel(AdminApiService apiService, Action navigateBack)
    {
        _apiService = apiService;
        _navigateBack = navigateBack;
    }

    public async Task LoadAsync(int customerId)
    {
        _customerId = customerId;
        IsLoading = true;
        try
        {
            var detail = await _apiService.GetCustomerAsync(customerId);
            if (detail == null) return;

            CustomerName = detail.FullName;
            Nik = detail.NIK;
            Phone = detail.Phone;
            Email = detail.Email ?? "-";
            Address = detail.Address ?? "-";
            DateOfBirth = detail.DateOfBirth.ToString("dd MMM yyyy");
            CreatedAt = detail.CreatedAt.ToString("dd MMM yyyy HH:mm");
            Accounts = new ObservableCollection<AccountDto>(detail.Accounts);
            Cards = new ObservableCollection<CardDto>(detail.Cards);

            // Load account types for the "Open Account" dropdown
            var types = await _apiService.GetAccountTypesAsync();
            AccountTypes = new ObservableCollection<AccountTypeDto>(types);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task OpenAccountAsync()
    {
        if (SelectedAccountType == null)
        {
            StatusMessage = "Pilih jenis rekening terlebih dahulu.";
            return;
        }

        try
        {
            var request = new CreateAccountRequest(_customerId, SelectedAccountType.AccountTypeId, InitialDeposit);
            var account = await _apiService.CreateAccountAsync(request);
            if (account != null) Accounts.Add(account);
            StatusMessage = $"Rekening {account?.AccountNumber} berhasil dibuat!";
            InitialDeposit = 0;
            SelectedAccountType = null;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Gagal: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task IssueCardAsync()
    {
        if (SelectedAccountForCard == null)
        {
            StatusMessage = "Pilih rekening untuk kartu.";
            return;
        }
        if (string.IsNullOrWhiteSpace(NewCardPin) || NewCardPin.Length != 6)
        {
            StatusMessage = "PIN harus 6 digit.";
            return;
        }

        try
        {
            var request = new CreateCardRequest(_customerId, SelectedAccountForCard.AccountId, NewCardPin);
            var card = await _apiService.CreateCardAsync(request);
            if (card != null) Cards.Add(card);
            StatusMessage = $"Kartu {card?.CardNumber} berhasil diterbitkan!";
            NewCardPin = string.Empty;
            SelectedAccountForCard = null;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Gagal: {ex.Message}";
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigateBack();
    }
}
