using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Atm.AppLayer.Services;
using System.Windows;
using System.Collections.ObjectModel;

namespace BankingEcosystem.Atm.UI.ViewModels;

public partial class WithdrawViewModel : ObservableObject
{
    private readonly ITransactionService _transactionService;
    private readonly AtmSessionService _sessionService;

    public enum WithdrawStep
    {
        SelectAmount,
        CustomAmount,
        Processing,
        Result
    }

    [ObservableProperty]
    private WithdrawStep _currentStep = WithdrawStep.SelectAmount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(KeypadValueDisplay))]
    [NotifyPropertyChangedFor(nameof(CustomAmountDisplay))]
    private decimal _customAmount = 0;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy = false;

    [ObservableProperty]
    private string _resultTitle = string.Empty;

    [ObservableProperty]
    private string _resultMessage = string.Empty;

    public string KeypadValueDisplay => CustomAmount == 0 ? "" : CustomAmount.ToString("N0");
    public string CustomAmountDisplay => $"Rp {CustomAmount:N0}";

    public WithdrawViewModel(ITransactionService transactionService, AtmSessionService sessionService)
    {
        _transactionService = transactionService;
        _sessionService = sessionService;
    }

    public event Action? RequestClose;

    [RelayCommand]
    private async Task OnSelectAmount(string amountStr)
    {
        if (IsBusy) return;
        if (decimal.TryParse(amountStr, out decimal amount))
        {
            await ExecuteWithdraw(amount);
        }
    }

    [RelayCommand]
    private void OnOtherAmount()
    {
        if (IsBusy) return;
        CustomAmount = 0;
        StatusMessage = "";
        CurrentStep = WithdrawStep.CustomAmount;
    }

    [RelayCommand]
    private void OnDigit(string digit)
    {
        if (IsBusy || CurrentStep != WithdrawStep.CustomAmount) return;

        if (CustomAmount < 10_000_000) // Max limit check
        {
             try 
             {
                 string current = CustomAmount.ToString("F0");
                 if (current == "0") current = "";
                 string next = current + digit;
                 if (decimal.TryParse(next, out decimal val))
                 {
                     CustomAmount = val;
                 }
             }
             catch { }
        }
    }

    [RelayCommand]
    private void OnBackspace()
    {
        if (IsBusy || CurrentStep != WithdrawStep.CustomAmount) return;

        string current = CustomAmount.ToString("F0");
        if (current.Length > 0)
        {
            current = current[..^1];
            if (current.Length == 0) CustomAmount = 0;
            else 
            {
                if (decimal.TryParse(current, out decimal val)) CustomAmount = val;
            }
        }
        else CustomAmount = 0;
    }

    [RelayCommand]
    private void OnClear()
    {
        if (IsBusy || CurrentStep != WithdrawStep.CustomAmount) return;
        CustomAmount = 0;
    }

    [RelayCommand]
    private async Task OnEnter()
    {
        if (IsBusy) return;

        if (CurrentStep == WithdrawStep.CustomAmount)
        {
            if (CustomAmount <= 0)
            {
                StatusMessage = "Jumlah harus lebih besar dari 0";
                return;
            }
            if (CustomAmount % 50000 != 0)
            {
                 StatusMessage = "Kelipatan Rp 50.000";
                 return;
            }
            await ExecuteWithdraw(CustomAmount);
        }
        else if (CurrentStep == WithdrawStep.Result)
        {
            RequestClose?.Invoke();
        }
    }

    [RelayCommand]
    private void OnCancel()
    {
        if (IsBusy) return;
        if (CurrentStep == WithdrawStep.CustomAmount)
        {
            CurrentStep = WithdrawStep.SelectAmount;
        }
        else
        {
            RequestClose?.Invoke();
        }
    }

    private async Task ExecuteWithdraw(decimal amount)
    {
        IsBusy = true;
        StatusMessage = "Memproses Penarikan...";
        CurrentStep = WithdrawStep.Processing;

        // Call Service
        var result = await _transactionService.WithdrawAsync(amount);

        IsBusy = false;
        
        if (result == "Success")
        {
            ResultTitle = "Penarikan Berhasil";
            ResultMessage = $"Silakan ambil uang Anda sebesar\nRp {amount:N0}";
        }
        else
        {
            ResultTitle = "Penarikan Gagal";
            ResultMessage = result;
        }
        
        CurrentStep = WithdrawStep.Result;
    }
}
