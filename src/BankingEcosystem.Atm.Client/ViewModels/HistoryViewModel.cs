using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Atm.AppLayer.Services;
using BankingEcosystem.Shared.DTOs;
using System.Collections.ObjectModel;

namespace BankingEcosystem.Atm.UI.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly ITransactionService _transactionService;
    private readonly AtmSessionService _sessionService;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public ObservableCollection<TransactionDto> Transactions { get; } = new();

    public HistoryViewModel(ITransactionService transactionService, AtmSessionService sessionService)
    {
        _transactionService = transactionService;
        _sessionService = sessionService;
    }

    public async Task LoadHistoryAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = "";
        Transactions.Clear();

        try
        {
            // Fetch last 10 transactions for Mini Statement
            var results = await _transactionService.GetHistoryAsync(10);
            
            if (results == null || results.Count == 0)
            {
                ErrorMessage = "Belum ada riwayat transaksi.";
            }
            else
            {
                foreach (var tx in results)
                {
                    Transactions.Add(tx);
                }
            }
        }
        catch
        {
            ErrorMessage = "Gagal memuat riwayat transaksi.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Navigation event
    public event Action? RequestClose;

    [RelayCommand]
    private void OnBack()
    {
        RequestClose?.Invoke();
    }
}
