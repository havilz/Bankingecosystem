using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Atm.AppLayer.Services;
using System.Windows;

namespace BankingEcosystem.Atm.UI.ViewModels;

public partial class TransferViewModel : ObservableObject
{
    private readonly ITransactionService _transactionService;
    private readonly AtmSessionService _sessionService;

    public enum TransferStep
    {
        InputAccount,
        InputAmount,
        Confirm,
        Processing,
        Result
    }

    [ObservableProperty]
    private TransferStep _currentStep = TransferStep.InputAccount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(KeypadValueDisplay))]
    private string _targetAccountNumber = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(KeypadValueDisplay))]
    [NotifyPropertyChangedFor(nameof(AmountDisplay))]
    private decimal _amount = 0;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy = false;

    [ObservableProperty]
    private string _resultTitle = string.Empty;

    [ObservableProperty]
    private string _resultMessage = string.Empty;

    public string KeypadValueDisplay => CurrentStep == TransferStep.InputAccount 
        ? TargetAccountNumber 
        : CurrentStep == TransferStep.InputAmount 
            ? (Amount == 0 ? "" : Amount.ToString("N0")) 
            : "";

    public string AmountDisplay => $"Rp {Amount:N0}";

    public TransferViewModel(ITransactionService transactionService, AtmSessionService sessionService)
    {
        _transactionService = transactionService;
        _sessionService = sessionService;
    }

    // Event to request navigation (handled by View)
    public event Action? RequestClose;

    [RelayCommand]
    private void OnDigit(string digit)
    {
        if (IsBusy) return;

        if (CurrentStep == TransferStep.InputAccount)
        {
            if (TargetAccountNumber.Length < 16) // Max account length
            {
                TargetAccountNumber += digit;
            }
        }
        else if (CurrentStep == TransferStep.InputAmount)
        {
            // Simple logic: append digit to decimal? No, input as string then parse.
            // Or simpler: Treat as integer input.
            // If user types '5', amount = 5. Types '0', amount = 50.
            if (Amount < 100_000_000) // Max limit check
            {
                 try 
                 {
                     string current = Amount.ToString("F0");
                     if (current == "0") current = "";
                     string next = current + digit;
                     if (decimal.TryParse(next, out decimal val))
                     {
                         Amount = val;
                     }
                 }
                 catch { }
            }
        }
    }

    [RelayCommand]
    private void OnClear()
    {
        if (IsBusy) return;

        if (CurrentStep == TransferStep.InputAccount)
        {
            TargetAccountNumber = "";
        }
        else if (CurrentStep == TransferStep.InputAmount)
        {
            Amount = 0;
        }
    }
    
    [RelayCommand]
    private void OnBackspace()
    {
        if (IsBusy) return;

        if (CurrentStep == TransferStep.InputAccount)
        {
            if (TargetAccountNumber.Length > 0)
                TargetAccountNumber = TargetAccountNumber[..^1];
        }
        else if (CurrentStep == TransferStep.InputAmount)
        {
             // Remove last digit
             string current = Amount.ToString("F0");
             if (current.Length > 0)
             {
                 current = current[..^1];
                 if (current.Length == 0) amount = 0;
                 else decimal.TryParse(current, out amount);
                 Amount = amount;
             }
             else Amount = 0;
        }
    }

    private decimal amount; // Helper

    [RelayCommand]
    private async Task OnEnter()
    {
        if (IsBusy) return;

        switch (CurrentStep)
        {
            case TransferStep.InputAccount:
                if (string.IsNullOrWhiteSpace(TargetAccountNumber))
                {
                    StatusMessage = "Masukkan nomor rekening tujuan";
                    return;
                }
                // TODO: Validate account existence? (Optional, maybe just proceed)
                StatusMessage = "";
                CurrentStep = TransferStep.InputAmount;
                break;

            case TransferStep.InputAmount:
                if (Amount <= 0)
                {
                    StatusMessage = "Jumlah harus lebih besar dari 0";
                    return;
                }
                StatusMessage = "";
                CurrentStep = TransferStep.Confirm;
                break;

            case TransferStep.Confirm:
                await ExecuteTransfer();
                break;

            case TransferStep.Result:
                RequestClose?.Invoke();
                break;
        }
    }

    [RelayCommand]
    private void OnCancel()
    {
        if (IsBusy) return;
        RequestClose?.Invoke();
    }

    private async Task ExecuteTransfer()
    {
        IsBusy = true;
        StatusMessage = "Memproses transaksi...";
        CurrentStep = TransferStep.Processing;

        // Simulate delay or waiting for backend
        var result = await _transactionService.TransferAsync(Amount, TargetAccountNumber);

        IsBusy = false;
        
        if (result == "Success")
        {
            ResultTitle = "Transaksi Berhasil";
            ResultMessage = $"Transfer sebesar {AmountDisplay}\nke {TargetAccountNumber}\nberhasil dilakukan.";
        }
        else
        {
            ResultTitle = "Transaksi Gagal";
            ResultMessage = result; // Error details
        }
        
        CurrentStep = TransferStep.Result;
    }
}
