using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Admin.UI.Services;

namespace BankingEcosystem.Admin.UI.ViewModels;

public partial class AddCustomerViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;
    private readonly Action _onSaved;
    private readonly Action _onCancel;

    [ObservableProperty] private string _fullName = string.Empty;
    [ObservableProperty] private string _nik = string.Empty;
    [ObservableProperty] private string _phone = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private DateTime _dateOfBirth = new(2000, 1, 1);

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public AddCustomerViewModel(AdminApiService apiService, Action onSaved, Action onCancel)
    {
        _apiService = apiService;
        _onSaved = onSaved;
        _onCancel = onCancel;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(FullName))
        { ErrorMessage = "Nama lengkap wajib diisi."; return; }
        if (string.IsNullOrWhiteSpace(Nik) || Nik.Length != 16)
        { ErrorMessage = "NIK harus 16 digit."; return; }
        if (string.IsNullOrWhiteSpace(Phone))
        { ErrorMessage = "Nomor telepon wajib diisi."; return; }

        IsBusy = true;
        try
        {
            var request = new CreateCustomerRequest(FullName, Nik, Phone,
                string.IsNullOrWhiteSpace(Email) ? null : Email,
                string.IsNullOrWhiteSpace(Address) ? null : Address,
                DateOfBirth);

            await _apiService.CreateCustomerAsync(request);
            _onSaved();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
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
