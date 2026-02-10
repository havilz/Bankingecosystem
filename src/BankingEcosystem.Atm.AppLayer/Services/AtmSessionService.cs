using System;

namespace BankingEcosystem.Atm.AppLayer.Services;

public class AtmSessionService
{
    public event EventHandler? SessionExpired;

    public string? CurrentCardNumber { get; set; }
    public string? CurrentAuthToken { get; set; } // JWT from backend
    public string? CustomerName { get; set; }
    public int? AccountId { get; set; }
    public int? CardId { get; set; }
    public int AtmId { get; set; } = 1;
    
    public bool IsAuthenticated => !string.IsNullOrEmpty(CurrentAuthToken);

    private readonly System.Timers.Timer _sessionTimer;
    private const double SessionTimeoutSeconds = 60; // 60 seconds inactivity timeout

    public AtmSessionService()
    {
        _sessionTimer = new System.Timers.Timer(TimeSpan.FromSeconds(SessionTimeoutSeconds));
        _sessionTimer.AutoReset = false; // Run only once, then stop
        _sessionTimer.Elapsed += OnSessionTimerElapsed;
    }

    public void StartSession(string cardNumber)
    {
        CurrentCardNumber = cardNumber;
        CurrentAuthToken = null;
        CustomerName = null;
        AccountId = null;
        CardId = null;
        
        RefreshSession(); // Start the timer
    }

    public void Authenticate(string token, string customerName, int? accountId)
    {
        CurrentAuthToken = token;
        CustomerName = customerName;
        AccountId = accountId;
        
        RefreshSession();
    }

    public void RefreshSession()
    {
        if (string.IsNullOrEmpty(CurrentCardNumber)) return;

        _sessionTimer.Stop();
        _sessionTimer.Start();
    }

    public void ClearSession()
    {
        _sessionTimer.Stop();
        
        CurrentCardNumber = null;
        CurrentAuthToken = null;
        CustomerName = null;
        AccountId = null;
        CardId = null;
    }

    public void EndSession()
    {
        ClearSession();
    }

    private void OnSessionTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        ClearSession();
        SessionExpired?.Invoke(this, EventArgs.Empty);
    }
}
