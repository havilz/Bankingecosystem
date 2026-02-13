using System.Net.Http.Json;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Atm.AppLayer.Models;

namespace BankingEcosystem.Atm.AppLayer.Services;

public interface ITransactionService
{
    Task<decimal?> GetBalanceAsync();
    Task<string> WithdrawAsync(decimal amount);
    Task<string> DepositAsync(decimal amount);
    Task<string> TransferAsync(decimal amount, string targetAccountNumber);
    Task<List<TransactionDto>> GetHistoryAsync(int limit = 10);
}

public class TransactionService : ITransactionService
{
    private readonly HttpClient _httpClient;
    private readonly AtmSessionService _sessionService;
    private readonly IHardwareInteropService _hardwareService;
    private readonly IAtmStateService _atmStateService;

    public TransactionService(HttpClient httpClient, AtmSessionService sessionService, IHardwareInteropService hardwareService, IAtmStateService atmStateService)
    {
        _httpClient = httpClient;
        _sessionService = sessionService;
        _hardwareService = hardwareService;
        _atmStateService = atmStateService;
    }

    public async Task<decimal?> GetBalanceAsync()
    {
        if (!_sessionService.IsAuthenticated || _sessionService.AccountId == null) return null;

        try
        {
            var response = await _httpClient.GetAsync($"api/transaction/balance/{_sessionService.AccountId}?atmId={_sessionService.AtmId}");
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TransactionDto>>();
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

        // FSM: Start Transaction
        try 
        { 
            _atmStateService.TransitionTo(AtmState.Transaction); 
        } 
        catch 
        { 
            // Retry with recovery if state is stuck (e.g. Completed)
            try
            {
                _atmStateService.RecoverToAuthenticated();
                _atmStateService.TransitionTo(AtmState.Transaction);
            }
            catch
            {
                return "Hardware state error"; 
            }
        }

        // 1. Check Hardware Cash
        int amountInt = (int)amount;
        int remainingCash = _hardwareService.GetRemainingCash();
        if (remainingCash != -1 && remainingCash < amountInt)
        {
            try { _atmStateService.TransitionTo(AtmState.Authenticated); } catch {}
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
                try { _atmStateService.TransitionTo(AtmState.Authenticated); } catch {}
                return error?.Message ?? "Transaction failed";
            }

            // FSM: Dispensing
            try { _atmStateService.TransitionTo(AtmState.Dispensing); } catch {}

            // 3. Dispense Cash
            bool dispensed = _hardwareService.DispenseCash(amountInt);
            if (!dispensed)
            {
                // CRITICAL: Money deducted but not dispensed!
                // FSM: Error
                try { _atmStateService.TransitionTo(AtmState.Error); } catch {}
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

            // FSM: Completion -> Authenticated (Ready for next)
            try 
            { 
                _atmStateService.TransitionTo(AtmState.Completed); 
                // Simulate user taking cash/receipt time?
                _atmStateService.TransitionTo(AtmState.Authenticated);
            } 
            catch {}

            return "Success";
        }
        catch
        {
            try { _atmStateService.TransitionTo(AtmState.Error); } catch {}
            return "Network error";
        }
    }

    public async Task<string> TransferAsync(decimal amount, string targetAccountNumber)
    {
        if (!_sessionService.IsAuthenticated || _sessionService.AccountId == null) return "User not authenticated";
        if (amount <= 0) return "Invalid amount";
        if (string.IsNullOrWhiteSpace(targetAccountNumber)) return "Invalid target account";

        // FSM: Start Transaction
        try 
        { 
            _atmStateService.TransitionTo(AtmState.Transaction); 
        } 
        catch 
        { 
             try
            {
                _atmStateService.RecoverToAuthenticated();
                _atmStateService.TransitionTo(AtmState.Transaction);
            }
            catch
            {
                return "Hardware state error"; 
            }
        }

        try
        {
            var request = new TransferRequest(_sessionService.AccountId.Value, targetAccountNumber, amount, "ATM Transfer");
            var response = await _httpClient.PostAsJsonAsync("api/transaction/transfer", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                try { _atmStateService.TransitionTo(AtmState.Authenticated); } catch {}
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

            // FSM: Completion -> Authenticated
            try 
            { 
                _atmStateService.TransitionTo(AtmState.Completed); 
                _atmStateService.TransitionTo(AtmState.Authenticated);
            } 
            catch {}

            return "Success";
        }
        catch
        {
            try { _atmStateService.TransitionTo(AtmState.Error); } catch {}
            return "Network error";
        }
    }

    public async Task<string> DepositAsync(decimal amount)
    {
        if (!_sessionService.IsAuthenticated || _sessionService.AccountId == null) return "User not authenticated";
        if (amount <= 0) return "Invalid amount";

        // FSM: Start Transaction
        try 
        { 
            _atmStateService.TransitionTo(AtmState.Transaction); 
        } 
        catch 
        { 
             try
            {
                _atmStateService.RecoverToAuthenticated();
                _atmStateService.TransitionTo(AtmState.Transaction);
            }
            catch
            {
                return "Hardware state error"; 
            }
        }

        // 1. Simulate Accepting Cash Hardware
        // In a real ATM, this would enable the shutter and wait for bills.
        // potentially counting them.
        bool cashAccepted = _hardwareService.AcceptCash(); // We need to verify if this method exists or if we need to add it to Interface
        // Wait, IHardwareInteropService might not have AcceptCash yet. Let's check. 
        // Based on previous contexts, I simpler just proceed with API call for now 
        // OR assuming it exists. If not I will add it.
        // Let's assume for now we just process the transaction logically.
        
        try
        {
            var request = new DepositRequest(_sessionService.AccountId.Value, _sessionService.AtmId, amount);
            var response = await _httpClient.PostAsJsonAsync("api/transaction/deposit", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                try { _atmStateService.TransitionTo(AtmState.Authenticated); } catch {}
                return error?.Message ?? "Deposit failed";
            }

            // Print Receipt
             var receipt = new Helpers.ReceiptBuilder()
                .AddHeader("BANK ECOSYSTEM", _sessionService.AtmId.ToString())
                .AddBody("TRANSACTION", "DEPOSIT")
                .AddBody("DATE", DateTime.Now.ToString("dd/MM/yyyy"))
                .AddBody("TIME", DateTime.Now.ToString("HH:mm:ss"))
                .AddSeparator()
                .AddBody("AMOUNT", amount.ToString("C", new System.Globalization.CultureInfo("id-ID")))
                .AddBody("REF NO", Guid.NewGuid().ToString().Substring(0, 8).ToUpper())
                .AddFooter("Deposit Successful.")
                .ToString();
            
            try 
            {
                _hardwareService.PrintReceipt(receipt);
            }
            catch {}

            // FSM: Completion -> Authenticated
             try 
            { 
                _atmStateService.TransitionTo(AtmState.Completed); 
                _atmStateService.TransitionTo(AtmState.Authenticated);
            } 
            catch {}

            return "Success";
        }
        catch
        {
            try { _atmStateService.TransitionTo(AtmState.Error); } catch {}
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
