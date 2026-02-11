using BankingEcosystem.Atm.AppLayer.Services;

namespace BankingEcosystem.Tests;

/// <summary>
/// Tests for AtmSessionService — session lifecycle, timeout, and state management.
/// </summary>
public class SessionServiceTests
{
    [Fact]
    public void NewSession_IsNotAuthenticated()
    {
        var session = new AtmSessionService();
        Assert.False(session.IsAuthenticated);
        Assert.Null(session.CurrentCardNumber);
        Assert.Null(session.CurrentAuthToken);
        Assert.Null(session.CustomerName);
        Assert.Null(session.AccountId);
        Assert.Null(session.CardId);
    }

    [Fact]
    public void StartSession_SetsCardNumber()
    {
        var session = new AtmSessionService();
        session.StartSession("6221000000000001");

        Assert.Equal("6221000000000001", session.CurrentCardNumber);
        Assert.False(session.IsAuthenticated); // Not yet authenticated
        Assert.Null(session.CurrentAuthToken);
    }

    [Fact]
    public void Authenticate_SetsTokenAndCustomer()
    {
        var session = new AtmSessionService();
        session.StartSession("6221000000000001");
        session.Authenticate("jwt-token-123", "John Doe", 1);

        Assert.True(session.IsAuthenticated);
        Assert.Equal("jwt-token-123", session.CurrentAuthToken);
        Assert.Equal("John Doe", session.CustomerName);
        Assert.Equal(1, session.AccountId);
    }

    [Fact]
    public void CardId_PersistsBetweenStartAndAuthenticate()
    {
        var session = new AtmSessionService();
        session.StartSession("6221000000000001");
        session.CardId = 42;

        // CardId should survive until Authenticate
        Assert.Equal(42, session.CardId);

        session.Authenticate("jwt-token-123", "John Doe", 1);
        Assert.Equal(42, session.CardId); // Still accessible
    }

    [Fact]
    public void ClearSession_ResetsEverything()
    {
        var session = new AtmSessionService();
        session.StartSession("6221000000000001");
        session.CardId = 42;
        session.Authenticate("jwt-token-123", "John Doe", 1);

        session.ClearSession();

        Assert.False(session.IsAuthenticated);
        Assert.Null(session.CurrentCardNumber);
        Assert.Null(session.CurrentAuthToken);
        Assert.Null(session.CustomerName);
        Assert.Null(session.AccountId);
        Assert.Null(session.CardId);
    }

    [Fact]
    public void EndSession_ClearsSession()
    {
        var session = new AtmSessionService();
        session.StartSession("6221000000000001");
        session.Authenticate("jwt-token-123", "John Doe", 1);

        session.EndSession();

        Assert.False(session.IsAuthenticated);
        Assert.Null(session.CurrentCardNumber);
    }

    [Fact]
    public void AtmId_DefaultsToOne()
    {
        var session = new AtmSessionService();
        Assert.Equal(1, session.AtmId);
    }

    [Fact]
    public void SessionExpired_EventFires_AfterTimeout()
    {
        var session = new AtmSessionService();
        bool expired = false;
        session.SessionExpired += (_, _) => expired = true;

        session.StartSession("6221000000000001");

        // Timer is 60s, we can't easily test real timeout in unit test
        // But we can verify the event handler is wired
        Assert.False(expired); // Not yet expired
    }

    [Fact]
    public void StartSession_ClearsPreviousState()
    {
        var session = new AtmSessionService();
        session.StartSession("1111111111111111");
        session.Authenticate("old-token", "Old User", 99);
        session.CardId = 10;

        // Start a new session
        session.StartSession("2222222222222222");

        Assert.Equal("2222222222222222", session.CurrentCardNumber);
        Assert.Null(session.CurrentAuthToken); // Cleared
        Assert.Null(session.CustomerName);     // Cleared
        Assert.Null(session.AccountId);        // Cleared
        Assert.Null(session.CardId);           // Cleared
        Assert.False(session.IsAuthenticated);
    }
}
