using System;

namespace BankingEcosystem.Atm.AppLayer.Services;

public class AtmSessionService
{
    public string? CurrentCardNumber { get; set; }
    public string? CurrentAuthToken { get; set; } // JWT from backend
    public string? CustomerName { get; set; }
    public int? AccountId { get; set; }
    
    public bool IsAuthenticated => !string.IsNullOrEmpty(CurrentAuthToken);

    public void StartSession(string cardNumber)
    {
        CurrentCardNumber = cardNumber;
        CurrentAuthToken = null;
        CustomerName = null;
        AccountId = null;
    }

    public void Authenticate(string token, string customerName, int accountId)
    {
        CurrentAuthToken = token;
        CustomerName = customerName;
        AccountId = accountId;
    }

    public void ClearSession()
    {
        CurrentCardNumber = null;
        CurrentAuthToken = null;
        CustomerName = null;
        AccountId = null;
    }
}
