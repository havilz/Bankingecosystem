using BankingEcosystem.Atm.AppLayer.Services;

namespace BankingEcosystem.Tests;

/// <summary>
/// Tests for TransactionService — validates business logic
/// without making real HTTP calls (uses mock-like checks).
/// </summary>
public class TransactionServiceTests
{
    private static AtmSessionService CreateAuthenticatedSession()
    {
        var session = new AtmSessionService();
        session.StartSession("6221453754749809");
        session.CardId = 1;
        session.Authenticate("test-jwt-token", "John Doe", 1);
        return session;
    }

    private static AtmSessionService CreateUnauthenticatedSession()
    {
        return new AtmSessionService();
    }

    // ─── GetBalanceAsync Tests ───

    [Fact]
    public async Task GetBalance_ReturnsNull_WhenNotAuthenticated()
    {
        var session = CreateUnauthenticatedSession();
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:1/") }; // Dummy
        var hardware = new TestHardwareService();
        var service = new TransactionService(httpClient, session, hardware);

        var result = await service.GetBalanceAsync();
        Assert.Null(result); // Should fail fast without calling API
    }

    [Fact]
    public async Task GetBalance_ReturnsNull_WhenAccountIdMissing()
    {
        var session = new AtmSessionService();
        session.StartSession("6221453754749809");
        // Authenticated but no AccountId
        session.Authenticate("token", "User", null);

        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:1/") };
        var hardware = new TestHardwareService();
        var service = new TransactionService(httpClient, session, hardware);

        var result = await service.GetBalanceAsync();
        Assert.Null(result);
    }

    // ─── WithdrawAsync Tests ───

    [Fact]
    public async Task Withdraw_ReturnsError_WhenNotAuthenticated()
    {
        var session = CreateUnauthenticatedSession();
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:1/") };
        var hardware = new TestHardwareService();
        var service = new TransactionService(httpClient, session, hardware);

        var result = await service.WithdrawAsync(100000);
        Assert.Equal("User not authenticated", result);
    }

    [Fact]
    public async Task Withdraw_ReturnsError_WhenAmountZero()
    {
        var session = CreateAuthenticatedSession();
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:1/") };
        var hardware = new TestHardwareService();
        var service = new TransactionService(httpClient, session, hardware);

        var result = await service.WithdrawAsync(0);
        Assert.Equal("Invalid amount", result);
    }

    [Fact]
    public async Task Withdraw_ReturnsError_WhenAmountNegative()
    {
        var session = CreateAuthenticatedSession();
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:1/") };
        var hardware = new TestHardwareService();
        var service = new TransactionService(httpClient, session, hardware);

        var result = await service.WithdrawAsync(-50000);
        Assert.Equal("Invalid amount", result);
    }

    [Fact]
    public async Task Withdraw_ReturnsError_WhenInsufficientATMCash()
    {
        var session = CreateAuthenticatedSession();
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:1/") };
        var hardware = new TestHardwareService(remainingCash: 50000); // Only 50k left
        var service = new TransactionService(httpClient, session, hardware);

        var result = await service.WithdrawAsync(100000); // Requesting 100k
        Assert.Equal("ATM insufficient cash", result);
    }

    [Fact]
    public async Task Withdraw_ReturnsNetworkError_WhenApiFails()
    {
        var session = CreateAuthenticatedSession();
        // Invalid URL will cause connection refused
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:1/") };
        var hardware = new TestHardwareService(remainingCash: 10000000);
        var service = new TransactionService(httpClient, session, hardware);

        var result = await service.WithdrawAsync(100000);
        Assert.Equal("Network error", result);
    }

    // ─── Test Helpers ───

    /// <summary>
    /// Simple test implementation of IHardwareInteropService.
    /// No real DLL calls.
    /// </summary>
    private class TestHardwareService : IHardwareInteropService
    {
        private readonly int _remainingCash;
        private readonly bool _dispenseResult;

        public TestHardwareService(int remainingCash = -1, bool dispenseResult = true)
        {
            _remainingCash = remainingCash;
            _dispenseResult = dispenseResult;
        }

        public string? ReadCard() => "6221453754749809";
        public bool EjectCard() => true;
        public bool IsCardInserted() => true;
        public bool DispenseCash(int amount) => _dispenseResult;
        public bool PrintReceipt(string content) => true;
        public int GetRemainingCash() => _remainingCash;
    }
}
