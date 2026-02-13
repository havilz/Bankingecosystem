using System.Net.Http.Json;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Atm.AppLayer.Services;

public interface ITransactionService
{
    Task<decimal?> GetBalanceAsync();
    Task<string> WithdrawAsync(decimal amount);
    Task<string> TransferAsync(decimal amount, string targetAccountNumber);
    Task<List<TransactionDto>> GetHistoryAsync(int limit = 10);
}

public class TransactionService : ITransactionService
{
    private readonly HttpClient _httpClient;
    private readonly AtmSessionService _sessionService;
    private readonly IHardwareInteropService _hardwareService;

    public TransactionService(HttpClient httpClient, AtmSessionService sessionService, IHardwareInteropService hardwareService)
    {
        _httpClient = httpClient;
        _sessionService = sessionService;
        _hardwareService = hardwareService;
    }

    public async Task<decimal?> GetBalanceAsync()
    {
        if (!_sessionService.IsAuthenticated || _sessionService.AccountId == null) return null;

        try
        {
            var response = await _httpClient.GetAsync($"api/transaction/balance/{_sessionService.AccountId}?atmId={_sessionService.AtmId}");
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TransactionDto>>();
            // BalanceInquiry returns a TransactionDto where BalanceAfter is the current balance?
            // Or maybe we should check AccountController for simpler balance?
            // "api/account/{id}" returns AccountDto with Balance.
            // But checking transaction history/balance inquiry usually logs a transaction.
            // Let's use the one we called.
            
            return result?.Data?.BalanceAfter;
        }
        catch
        {
            return null;
        }
    }

    public async Task<string> WithdrawAsync(decimal amount)
    {
        if (!_sessionService.IsAuthenticated || _sessionService.AccountId == null) return "User not authenticated";
        if (amount <= 0) return "Invalid amount";
        if (amount % 50000 != 0 && amount % 100000 != 0) return "Invalid denomination"; // Basic check

        // 1. Check Hardware Cash
        int amountInt = (int)amount;
        int remainingCash = _hardwareService.GetRemainingCash();
        if (remainingCash != -1 && remainingCash < amountInt)
        {
            return "ATM insufficient cash";
        }

        try
        {
            // 2. Call Backend to Debit (this will check account balance)
            var request = new WithdrawRequest(_sessionService.AccountId.Value, _sessionService.AtmId, amount);
            var response = await _httpClient.PostAsJsonAsync("api/transaction/withdraw", request);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                return error?.Message ?? "Transaction failed";
            }

            // 3. Dispense Cash
            bool dispensed = _hardwareService.DispenseCash(amountInt);
            if (!dispensed)
            {
                // CRITICAL: Money deducted but not dispensed!
                // In real world, we would call a Reversal API here.
                // For this simulation, we'll just return a specific error.
                return "Hardware error: Cash not dispensed. Please contact bank.";
            }

            // 4. Print Receipt
            var receipt = new Helpers.ReceiptBuilder()
                .AddHeader("BANK ECOSYSTEM", _sessionService.AtmId.ToString())
                .AddBody("TRANSACTION", "WITHDRAWAL")
                .AddBody("DATE", DateTime.Now.ToString("dd/MM/yyyy"))
                .AddBody("TIME", DateTime.Now.ToString("HH:mm:ss"))
                .AddSeparator()
                .AddBody("AMOUNT", amount.ToString("C", new System.Globalization.CultureInfo("id-ID")))
                .AddBody("REF NO", Guid.NewGuid().ToString().Substring(0, 8).ToUpper())
                .AddFooter("Keep your receipt safe.")
                .ToString();
            _hardwareService.PrintReceipt(receipt);

            return "Success";
        }
        catch
        {
            return "Network error";
        }
    }
    public async Task<string> TransferAsync(decimal amount, string targetAccountNumber)
    {
        if (!_sessionService.IsAuthenticated || _sessionService.AccountId == null) return "User not authenticated";
        if (amount <= 0) return "Invalid amount";
        if (string.IsNullOrWhiteSpace(targetAccountNumber)) return "Invalid target account";

        try
        {
            var request = new TransferRequest(_sessionService.AccountId.Value, targetAccountNumber, amount, "ATM Transfer");
            var response = await _httpClient.PostAsJsonAsync("api/transaction/transfer", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                return error?.Message ?? "Transfer failed";
            }

            // Print Receipt
            var receipt = new Helpers.ReceiptBuilder()
                .AddHeader("BANK ECOSYSTEM", _sessionService.AtmId.ToString())
                .AddBody("TRANSACTION", "TRANSFER")
                .AddBody("DATE", DateTime.Now.ToString("dd/MM/yyyy"))
                .AddBody("TIME", DateTime.Now.ToString("HH:mm:ss"))
                .AddSeparator()
                .AddBody("DEST ACCOUNT", targetAccountNumber)
                .AddBody("AMOUNT", amount.ToString("C", new System.Globalization.CultureInfo("id-ID")))
                .AddBody("REF NO", Guid.NewGuid().ToString().Substring(0, 8).ToUpper())
                .AddFooter("Transfer Successful.")
                .ToString();
            try 
            {
                _hardwareService.PrintReceipt(receipt);
            }
            catch 
            { 
                // Ignore printer errors for now
            }

            return "Success";
        }
        catch
        {
            return "Network error";
        }
    }

    public async Task<List<TransactionDto>> GetHistoryAsync(int limit = 10)
    {
        if (!_sessionService.IsAuthenticated || _sessionService.AccountId == null) return new List<TransactionDto>();

        try
        {
            var response = await _httpClient.GetAsync($"api/transaction/history/{_sessionService.AccountId}?pageSize={limit}");
            if (!response.IsSuccessStatusCode) return new List<TransactionDto>();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<TransactionDto>>>();
            return result?.Data ?? new List<TransactionDto>();
        }
        catch
        {
            return new List<TransactionDto>();
        }
    }
}
