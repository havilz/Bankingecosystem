using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Admin.UI.Services;

namespace BankingEcosystem.Admin.UI.ViewModels;

public partial class MbankingListViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;
    private List<MbankingAccountDto> _allAccounts = [];

    [ObservableProperty]
    private ObservableCollection<MbankingAccountDto> _accounts = [];

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private MbankingAccountDto? _selectedAccount;

    // Edit state
    [ObservableProperty]
    private string _editEmail = string.Empty;

    [ObservableProperty]
    private bool _isEditDialogOpen;

    public MbankingListViewModel(AdminApiService apiService)
    {
        _apiService = apiService;
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsLoading = true;
        try
        {
            _allAccounts = await _apiService.GetMbankingAccountsAsync();
            ApplyFilter();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Gagal memuat data akun mbanking:\n{ex.Message}", "Error",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private void OpenEdit(MbankingAccountDto account)
    {
        SelectedAccount = account;
        EditEmail = account.Email;
        IsEditDialogOpen = true;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditDialogOpen = false;
        SelectedAccount = null;
        EditEmail = string.Empty;
    }

    [RelayCommand]
    private async Task SaveEmail()
    {
        if (SelectedAccount is null || string.IsNullOrWhiteSpace(EditEmail)) return;
        IsLoading = true;
        try
        {
            await _apiService.UpdateMbankingEmailAsync(SelectedAccount.MbankingAccountId, EditEmail.Trim());
            System.Windows.MessageBox.Show("Email berhasil diperbarui.", "Sukses",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            IsEditDialogOpen = false;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Gagal memperbarui email:\n{ex.Message}", "Error",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task DeleteAccount(MbankingAccountDto account)
    {
        var result = System.Windows.MessageBox.Show(
            $"Hapus akun mbanking milik\n\"{account.CustomerName}\" ({account.Email})?\n\nAkun nasabah & kartu tidak akan terhapus.",
            "Konfirmasi Hapus",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning);

        if (result != System.Windows.MessageBoxResult.Yes) return;

        IsLoading = true;
        try
        {
            await _apiService.DeleteMbankingAccountAsync(account.MbankingAccountId);
            System.Windows.MessageBox.Show("Akun mbanking berhasil dihapus.", "Sukses",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Gagal menghapus:\n{ex.Message}", "Error",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
        finally { IsLoading = false; }
    }

    private void ApplyFilter()
    {
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allAccounts
            : _allAccounts.Where(a =>
                a.CustomerName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.AccountNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.CardNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        Accounts = new ObservableCollection<MbankingAccountDto>(filtered);
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
}
