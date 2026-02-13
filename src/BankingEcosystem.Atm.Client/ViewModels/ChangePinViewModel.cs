using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using BankingEcosystem.Atm.AppLayer.Services;

namespace BankingEcosystem.Atm.UI.ViewModels;

public partial class ChangePinViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly AtmSessionService _sessionService;
    private readonly IAtmStateService _atmStateService; // For FSM enforcement

    [ObservableProperty]
    private string _title = "Ubah PIN";

    [ObservableProperty]
    private string _instruction = "Masukkan PIN Lama Anda";

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    // State: 0=Old, 1=New, 2=Confirm
    private int _step = 0;
    
    private string _oldPin = string.Empty;
    private string _newPin = string.Empty;

    public ChangePinViewModel(IAuthService authService, AtmSessionService sessionService, IAtmStateService atmStateService)
    {
        _authService = authService;
        _sessionService = sessionService;
        _atmStateService = atmStateService;
        
        // Start in "Transaction" state? Or maybe just keep Authenticated?
        // Changing PIN is sensitive. Let's say it's a Transaction.
        // try { _atmStateService.TransitionTo(AtmState.Transaction); } catch {}
    }

    // Called by View when PIN is submitted (6 digits)
    public async Task ProcessPinAsync(string pin, Action resetPinCallback, Action closeCallback)
    {
        if (IsBusy) return;

        switch (_step)
        {
            case 0: // Old PIN
                await HandleOldPinAsync(pin, resetPinCallback);
                break;
            case 1: // New PIN
                HandleNewPin(pin, resetPinCallback);
                break;
            case 2: // Confirm PIN
                await HandleConfirmPinAsync(pin, resetPinCallback, closeCallback);
                break;
        }
    }

    private async Task HandleOldPinAsync(string pin, Action resetPinCallback)
    {
        IsBusy = true;
        StatusMessage = "Memverifikasi PIN Lama...";

        // Verify with backend (or local check if we had it, but we don't store hash in session for security)
        // Actually AuthService has VerifyPinAsync.
        // But VerifyPinAsync generates a token. We just want to check if it's correct.
        // We can reuse VerifyPinAsync, or assume if backend accepts the ChangePin request later, it's fine.
        // BUT, for better UX, we should verify OLD PIN *before* asking for NEW PIN.
        // Let's use VerifyPinAsync.
        
        if (_sessionService.CardId == null)
        {
            StatusMessage = "Error Sesi Invalid";
            IsBusy = false;
            return;
        }

        bool isValid = await _authService.VerifyPinAsync(_sessionService.CardId.Value, pin);
        IsBusy = false;

        if (isValid)
        {
            _oldPin = pin;
            _step = 1;
            Instruction = "Masukkan PIN Baru (6 Digit)";
            StatusMessage = string.Empty;
            resetPinCallback?.Invoke();
        }
        else
        {
            StatusMessage = "PIN Lama Salah!";
            resetPinCallback?.Invoke();
        }
    }

    private void HandleNewPin(string pin, Action resetPinCallback)
    {
        if (pin == _oldPin)
        {
            StatusMessage = "PIN Baru tidak boleh sama dengan PIN Lama";
            resetPinCallback?.Invoke();
            return;
        }

        _newPin = pin;
        _step = 2;
        Instruction = "Masukkan Ulang PIN Baru";
        StatusMessage = string.Empty;
        resetPinCallback?.Invoke();
    }

    private async Task HandleConfirmPinAsync(string confirmPin, Action resetPinCallback, Action closeCallback)
    {
        if (confirmPin != _newPin)
        {
            StatusMessage = "PIN Tidak Cocok! Coba lagi.";
            // Go back to step 1? Or just clear confirm?
            // Usually clear confirm and stay in step 2.
            resetPinCallback?.Invoke();
            return;
        }

        // Final Submit
        IsBusy = true;
        StatusMessage = "Memproses Perubahan PIN...";

        if (_sessionService.CardId == null)
        {
            StatusMessage = "Sesi Invalid";
            IsBusy = false;
            return;
        }

        bool success = await _authService.ChangePinAsync(_sessionService.CardId.Value, _oldPin, _newPin);
        IsBusy = false;

        if (success)
        {
            StatusMessage = "PIN Berhasil Diubah!";
            Instruction = "Sukses";
            await Task.Delay(1500);
            closeCallback?.Invoke();
        }
        else
        {
            StatusMessage = "Gagal Mengubah PIN (Sistem Error atau Blokir)";
            resetPinCallback?.Invoke();
            // Reset to step 0?
            _step = 0;
            Instruction = "Masukkan PIN Lama Anda";
        }
    }

    public void ResetState()
    {
        _step = 0;
        _oldPin = string.Empty;
        _newPin = string.Empty;
        Instruction = "Masukkan PIN Lama Anda";
        StatusMessage = string.Empty;
        IsBusy = false;
    }
}
