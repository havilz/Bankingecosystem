using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Backend.Models;
using BankingEcosystem.Backend.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Tests;

/// <summary>
/// Integration tests for Backend services using in-memory database.
/// Tests the real service logic against EF Core.
/// </summary>
public class BackendServiceTests : IDisposable
{
    private readonly BankingDbContext _db;

    public BackendServiceTests()
    {
        var options = new DbContextOptionsBuilder<BankingDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        _db = new BankingDbContext(options);

        // Seed required lookup data
        _db.AccountTypes.Add(new AccountType { AccountTypeId = 1, TypeName = "Savings", MinBalance = 50000, DefaultDailyLimit = 10000000 });
        _db.TransactionTypes.AddRange(
            new TransactionType { TransactionTypeId = 1, TypeName = "Withdrawal" },
            new TransactionType { TransactionTypeId = 2, TypeName = "Deposit" },
            new TransactionType { TransactionTypeId = 3, TypeName = "Transfer" },
            new TransactionType { TransactionTypeId = 4, TypeName = "BalanceInquiry" }
        );
        _db.Atms.Add(new BankingEcosystem.Backend.Models.Atm { AtmId = 1, AtmCode = "ATM001", Location = "Test Center", IsOnline = true, TotalCash = 50000000 });

        // Seed test customer + account + card
        _db.Customers.Add(new Customer
        {
            CustomerId = 1,
            FullName = "John Doe",
            NIK = "3201010101010001",
            Phone = "081234567890",
            Email = "john@test.com",
            Address = "Jakarta",
            DateOfBirth = new DateTime(1990, 1, 15)
        });
        _db.Accounts.Add(new Account
        {
            AccountId = 1,
            CustomerId = 1,
            AccountTypeId = 1,
            AccountNumber = "1234567890",
            Balance = 5000000,
            DailyLimit = 10000000,
            IsActive = true
        });
        _db.Cards.Add(new Card
        {
            CardId = 1,
            CustomerId = 1,
            AccountId = 1,
            CardNumber = "6221453754749809",
            PinHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            ExpiryDate = DateTime.UtcNow.AddYears(3),
            IsBlocked = false,
            FailedAttempts = 0
        });

        _db.SaveChanges();
    }

    public void Dispose()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    // ═══════════════════════════════════
    // Auth Service Tests
    // ═══════════════════════════════════

    [Fact]
    public async Task VerifyCard_ReturnsResponse_WhenCardExists()
    {
        var authService = CreateAuthService();
        var result = await authService.VerifyCardAsync("6221453754749809");

        Assert.NotNull(result);
        Assert.Equal(1, result!.CardId);
        Assert.Equal(1, result.AccountId);
        Assert.Equal("1234567890", result.AccountNumber);
        Assert.Equal("John Doe", result.CustomerName);
        Assert.False(result.IsBlocked);
    }

    [Fact]
    public async Task VerifyCard_ReturnsNull_WhenCardNotFound()
    {
        var authService = CreateAuthService();
        var result = await authService.VerifyCardAsync("9999999999999999");
        Assert.Null(result);
    }

    [Fact]
    public async Task VerifyPin_ReturnsAuthResponse_WhenCorrectPin()
    {
        var authService = CreateAuthService();
        var result = await authService.VerifyPinAsync(1, "123456");

        Assert.NotNull(result);
        Assert.NotEmpty(result!.Token);
        Assert.Equal("John Doe", result.CustomerName);
        Assert.Equal("1234567890", result.AccountNumber);
        Assert.Equal(5000000m, result.Balance);
        Assert.Equal(1, result.AccountId);
    }

    [Fact]
    public async Task VerifyPin_ReturnsNull_WhenWrongPin()
    {
        var authService = CreateAuthService();
        var result = await authService.VerifyPinAsync(1, "000000");
        Assert.Null(result);
    }

    [Fact]
    public async Task VerifyPin_IncrementsFailedAttempts_WhenWrongPin()
    {
        var authService = CreateAuthService();
        await authService.VerifyPinAsync(1, "000000");

        var card = await _db.Cards.FindAsync(1);
        Assert.Equal(1, card!.FailedAttempts);
    }

    [Fact]
    public async Task VerifyPin_BlocksCard_After3FailedAttempts()
    {
        var authService = CreateAuthService();

        await authService.VerifyPinAsync(1, "000000"); // Attempt 1
        await authService.VerifyPinAsync(1, "111111"); // Attempt 2
        await authService.VerifyPinAsync(1, "222222"); // Attempt 3 → blocked

        var card = await _db.Cards.FindAsync(1);
        Assert.True(card!.IsBlocked);
        Assert.Equal(3, card.FailedAttempts);
    }

    [Fact]
    public async Task VerifyPin_ResetsFailedAttempts_OnSuccess()
    {
        var authService = CreateAuthService();

        // Fail once
        await authService.VerifyPinAsync(1, "000000");
        var card = await _db.Cards.FindAsync(1);
        Assert.Equal(1, card!.FailedAttempts);

        // Then succeed
        await authService.VerifyPinAsync(1, "123456");
        await _db.Entry(card).ReloadAsync();
        Assert.Equal(0, card.FailedAttempts);
    }

    [Fact]
    public async Task VerifyPin_ReturnsNull_WhenCardBlocked()
    {
        var authService = CreateAuthService();

        // Block the card
        var card = await _db.Cards.FindAsync(1);
        card!.IsBlocked = true;
        await _db.SaveChangesAsync();

        var result = await authService.VerifyPinAsync(1, "123456"); // Correct PIN but blocked
        Assert.Null(result);
    }

    // ═══════════════════════════════════
    // Transaction Service Tests
    // ═══════════════════════════════════

    [Fact]
    public async Task BalanceInquiry_ReturnsCorrectBalance()
    {
        var txService = new TransactionService(_db);
        var result = await txService.BalanceInquiryAsync(1, 1);

        Assert.NotNull(result);
        Assert.Equal(5000000m, result.BalanceAfter);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public async Task BalanceInquiry_CreatesTransactionRecord()
    {
        var txService = new TransactionService(_db);
        await txService.BalanceInquiryAsync(1, 1);

        var txCount = await _db.Transactions.CountAsync();
        Assert.Equal(1, txCount);
    }

    [Fact]
    public async Task Withdraw_DeductsBalance()
    {
        var txService = new TransactionService(_db);
        var request = new WithdrawRequest(1, 1, 500000m);
        var result = await txService.WithdrawAsync(request);

        Assert.NotNull(result);
        Assert.Equal(500000m, result.Amount);
        Assert.Equal(5000000m, result.BalanceBefore);
        Assert.Equal(4500000m, result.BalanceAfter);
        Assert.Equal("Success", result.Status);

        var account = await _db.Accounts.FindAsync(1);
        Assert.Equal(4500000m, account!.Balance);
    }

    [Fact]
    public async Task Withdraw_ThrowsException_WhenInsufficientBalance()
    {
        var txService = new TransactionService(_db);
        var request = new WithdrawRequest(1, 1, 99999999m);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => txService.WithdrawAsync(request));
    }

    [Fact]
    public async Task Withdraw_MaintainsMinimumBalance()
    {
        var txService = new TransactionService(_db);
        // Try to withdraw 4,960,000 → would leave 40,000 which is < 50,000 minimum
        var request = new WithdrawRequest(1, 1, 4960000m);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => txService.WithdrawAsync(request));
    }

    [Fact]
    public async Task Deposit_IncreasesBalance()
    {
        var txService = new TransactionService(_db);
        var request = new DepositRequest(1, 1000000m);
        var result = await txService.DepositAsync(request);

        Assert.NotNull(result);
        Assert.Equal(1000000m, result.Amount);
        Assert.Equal(5000000m, result.BalanceBefore);
        Assert.Equal(6000000m, result.BalanceAfter);

        var account = await _db.Accounts.FindAsync(1);
        Assert.Equal(6000000m, account!.Balance);
    }

    [Fact]
    public async Task Deposit_ThrowsException_WhenAmountNegative()
    {
        var txService = new TransactionService(_db);
        var request = new DepositRequest(1, -500000m);

        await Assert.ThrowsAsync<ArgumentException>(
            () => txService.DepositAsync(request));
    }

    // ═══════════════════════════════════
    // Account Service Tests
    // ═══════════════════════════════════

    [Fact]
    public async Task GetAccountById_ReturnsCorrectAccount()
    {
        var accountService = new AccountService(_db);
        var result = await accountService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("1234567890", result!.AccountNumber);
        Assert.Equal(5000000m, result.Balance);
    }

    [Fact]
    public async Task GetAccountById_ReturnsNull_WhenNotFound()
    {
        var accountService = new AccountService(_db);
        var result = await accountService.GetByIdAsync(999);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAccountByNumber_ReturnsCorrectAccount()
    {
        var accountService = new AccountService(_db);
        var result = await accountService.GetByAccountNumberAsync("1234567890");

        Assert.NotNull(result);
        Assert.Equal(1, result!.AccountId);
    }

    [Fact]
    public async Task GetAccountsByCustomer_ReturnsAllAccounts()
    {
        var accountService = new AccountService(_db);
        var results = await accountService.GetByCustomerIdAsync(1);

        Assert.Single(results);
        Assert.Equal("1234567890", results[0].AccountNumber);
    }

    // ─── Helper ───

    private AuthService CreateAuthService()
    {
        var configDict = new Dictionary<string, string?>
        {
            { "Jwt:Key", "BankingEcosystemSuperSecretKey2026!@#$%^&*()" },
            { "Jwt:Issuer", "BankingEcosystem" },
            { "Jwt:Audience", "BankingEcosystemClient" }
        };
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(configDict)
            .Build();

        return new AuthService(_db, config);
    }
}
