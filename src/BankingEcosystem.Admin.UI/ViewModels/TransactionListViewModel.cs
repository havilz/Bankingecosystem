using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Admin.UI.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Admin.UI.ViewModels;

public partial class TransactionListViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<TransactionDto> _transactions = [];

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string? _selectedType;

    public ObservableCollection<string> TransactionTypes { get; } = ["", "Withdrawal", "Deposit", "Transfer", "BalanceInquiry"];

    [ObservableProperty]
    private int _page = 1;

    [ObservableProperty]
    private int _pageSize = 20;

    [ObservableProperty]
    private int _totalCount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanGoNext))]
    private int _totalPages;

    public bool CanGoPrevious => Page > 1;
    public bool CanGoNext => Page < TotalPages;

    public TransactionListViewModel(AdminApiService apiService)
    {
        _apiService = apiService;
        LoadTransactionsCommand.Execute(null);
    }

    partial void OnSearchTextChanged(string value) => LoadTransactionsCommand.Execute(null);
    partial void OnSelectedTypeChanged(string? value) => LoadTransactionsCommand.Execute(null);

    [RelayCommand]
    private async Task LoadTransactionsAsync()
    {
        if (IsLoading) return;
        IsLoading = true;
        Transactions.Clear();

        try
        {
            var result = await _apiService.GetTransactionsAsync(Page, PageSize, SearchText, SelectedType == "" ? null : SelectedType);
            if (result != null)
            {
                foreach (var item in result.Items) Transactions.Add(item);
                TotalCount = result.TotalCount;
                TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
                
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext));
            }
        }
        catch
        {
            // Handle error
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (CanGoNext)
        {
            Page++;
            await LoadTransactionsAsync();
        }
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (CanGoPrevious)
        {
            Page--;
            await LoadTransactionsAsync();
        }
    }
}
