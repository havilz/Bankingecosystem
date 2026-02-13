using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Atm.AppLayer.Services;
using System.Windows;

namespace BankingEcosystem.Atm.UI.ViewModels;

public partial class DepositViewModel : ObservableObject
{
    private readonly ITransactionService _transactionService;
    private readonly AtmSessionService _sessionService;

    public enum DepositStep
    {
        InputAmount,
        Processing,
        Result
    }

    [ObservableProperty]
    private DepositStep _currentStep = DepositStep.InputAmount;

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

    public string KeypadValueDisplay => Amount == 0 ? "" : Amount.ToString("N0");

    public string AmountDisplay => $"Rp {Amount:N0}";

    public DepositViewModel(ITransactionService transactionService, AtmSessionService sessionService)
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

        if (CurrentStep == DepositStep.InputAmount)
        {
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
        if (CurrentStep == DepositStep.InputAmount)
        {
            Amount = 0;
        }
    }
    
    [RelayCommand]
    private void OnBackspace()
    {
        if (IsBusy) return;

        if (CurrentStep == DepositStep.InputAmount)
        {
             // Remove last digit
             string current = Amount.ToString("F0");
             if (current.Length > 0)
             {
                 current = current[..^1];
                 if (current.Length == 0) Amount = 0;
                 else 
                 {
                     if (decimal.TryParse(current, out decimal val)) Amount = val;
                 }
             }
             else Amount = 0;
        }
    }

    [RelayCommand]
    private async Task OnEnter()
    {
        if (IsBusy) return;

        switch (CurrentStep)
        {
            case DepositStep.InputAmount:
                if (Amount <= 0)
                {
                    StatusMessage = "Jumlah harus lebih besar dari 0";
                    return;
                }
                if (Amount % 50000 != 0) // Basic validation for denom (optional)
                {
                     // Maybe warn but proceed? Or strict check?
                     // Let's enforce for realism
                     StatusMessage = "Kelipatan Rp 50.000";
                     return;
                }
                StatusMessage = "";
                await ExecuteDeposit();
                break;

            case DepositStep.Result:
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

    private async Task ExecuteDeposit()
    {
        IsBusy = true;
        StatusMessage = "Memproses Setor Tunai...";
        CurrentStep = DepositStep.Processing;

        // Call Service
        var result = await _transactionService.DepositAsync(Amount);

        IsBusy = false;
        
        if (result == "Success")
        {
            ResultTitle = "Setor Tunai Berhasil";
            ResultMessage = $"Setor tunai sebesar {AmountDisplay}\nberhasil ditambahkan ke rekening Anda.";
        }
        else
        {
            ResultTitle = "Setor Tunai Gagal";
            ResultMessage = result; // Error details
        }
        
        CurrentStep = DepositStep.Result;
    }
}
