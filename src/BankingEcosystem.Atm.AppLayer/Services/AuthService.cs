using System.Net.Http.Json;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Atm.AppLayer.Services;

public interface IAuthService
{
    Task<VerifyCardResponse?> VerifyCardAsync(string cardNumber);
    Task<bool> VerifyPinAsync(int cardId, string pin);
    Task<bool> ChangePinAsync(int cardId, string oldPin, string newPin);
}

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AtmSessionService _sessionService;
    private readonly IUiService _uiService;

    public AuthService(HttpClient httpClient, AtmSessionService sessionService, IUiService uiService)
    {
        _httpClient = httpClient;
        _sessionService = sessionService;
        _uiService = uiService;
    }

    public async Task<VerifyCardResponse?> VerifyCardAsync(string cardNumber)
    {
        _uiService.SetBusy(true);
        try
        {
            await Task.Delay(1500); // Realistic processing delay
            var response = await _httpClient.PostAsJsonAsync("api/auth/verify-card", new VerifyCardRequest(cardNumber));
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<VerifyCardResponse>>();
            return result?.Data; // Returns VerifyCardResponse if successful
        }
        catch
        {
            return null; // Network error or API down
        }
        finally
        {
            _uiService.SetBusy(false);
        }
    }

    public async Task<bool> VerifyPinAsync(int cardId, string pin)
    {
        _uiService.SetBusy(true);
        try
        {
            await Task.Delay(1500); // Realistic processing delay

            // Encrypt PIN before sending
            var encryptedPin = new System.Text.StringBuilder(64);
            BankingEcosystem.Interop.NativeLogicInterop.Encryption_EncryptPin(pin, encryptedPin, encryptedPin.Capacity);

            var response = await _httpClient.PostAsJsonAsync("api/auth/verify-pin", new VerifyPinRequest(cardId, encryptedPin.ToString()));
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
            if (result?.Success == true && result.Data != null)
            {
                _sessionService.Authenticate(result.Data.Token, result.Data.CustomerName, result.Data.AccountId);
                return true;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            try { File.AppendAllText("client_errors.log", $"[{DateTime.Now}] VerifyPinAsync Error: {ex}\n"); } catch { }
            return false;
        }
        finally
        {
            _uiService.SetBusy(false);
        }
    }

    public async Task<bool> ChangePinAsync(int cardId, string oldPin, string newPin)
    {
        _uiService.SetBusy(true);
        try
        {
            await Task.Delay(2000); // Realistic processing delay (longer for write op)

            // Encrypt PINs before sending
            var encryptedOld = new System.Text.StringBuilder(64);
            BankingEcosystem.Interop.NativeLogicInterop.Encryption_EncryptPin(oldPin, encryptedOld, encryptedOld.Capacity);

            var encryptedNew = new System.Text.StringBuilder(64);
            BankingEcosystem.Interop.NativeLogicInterop.Encryption_EncryptPin(newPin, encryptedNew, encryptedNew.Capacity);

             var response = await _httpClient.PostAsJsonAsync("api/auth/change-pin", new ChangePinRequest(cardId, encryptedOld.ToString(), encryptedNew.ToString()));
             if (!response.IsSuccessStatusCode) return false;

             var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
             return result?.Success ?? false;
        }
        catch (Exception ex)
        {
            try { File.AppendAllText("client_errors.log", $"[{DateTime.Now}] ChangePinAsync Error: {ex}\n"); } catch { }
            return false;
        }
        finally
        {
            _uiService.SetBusy(false);
        }
    }
}
