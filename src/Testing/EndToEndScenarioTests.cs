using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Backend.Models;
using BankingEcosystem.Backend.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Tests;

public class EndToEndScenarioTests : IDisposable
{
    private readonly BankingDbContext _db;

    public EndToEndScenarioTests()
    {
        var options = new DbContextOptionsBuilder<BankingDbContext>()
            .UseInMemoryDatabase(databaseName: $"E2E_TestDb_{Guid.NewGuid()}")
            .Options;
        _db = new BankingDbContext(options);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        // 1. Reference Data
        _db.AccountTypes.AddRange(
            new AccountType { AccountTypeId = 1, TypeName = "Savings", MinBalance = 50_000, DefaultDailyLimit = 10_000_000 },
            new AccountType { AccountTypeId = 2, TypeName = "Checking", MinBalance = 100_000, DefaultDailyLimit = 25_000_000 },
            new AccountType { AccountTypeId = 3, TypeName = "Business", MinBalance = 500_000, DefaultDailyLimit = 50_000_000 }
        );

        _db.TransactionTypes.AddRange(
            new TransactionType { TransactionTypeId = 1, TypeName = "Withdrawal" },
            new TransactionType { TransactionTypeId = 2, TypeName = "Deposit" },
            new TransactionType { TransactionTypeId = 3, TypeName = "Transfer" },
            new TransactionType { TransactionTypeId = 4, TypeName = "BalanceInquiry" }
        );

        _db.Atms.Add(new BankingEcosystem.Backend.Models.Atm 
        { 
            AtmId = 1, 
            AtmCode = "ATM001", 
            Location = "Head Office", 
            IsOnline = true, 
            TotalCash = 1_000_000_000 // Plenty of cash
        });

        // 2. Customers & Accounts

        // Budi: Normal User
        var budi = new Customer { CustomerId = 1, FullName = "Budi Santoso", NIK = "111", Phone = "081", CreatedAt = DateTime.Now };
        var budiAcc = new Account 
        { 
            AccountId = 1, 
            CustomerId = 1, 
            AccountTypeId = 1, // Savings
            AccountNumber = "1000000001", 
            Balance = 5_000_000, 
            DailyLimit = 10_000_000, 
            IsActive = true 
        };
        var budiCard = new Card 
        { 
            CardId = 1, 
            CustomerId = 1, 
            AccountId = 1, 
            CardNumber = "1111222233334444", 
            PinHash = BCrypt.Net.BCrypt.HashPassword("123456"), 
            IsBlocked = false,
            ExpiryDate = DateTime.Now.AddYears(3)
        };

        // Siti: Low Balance
        var siti = new Customer { CustomerId = 2, FullName = "Siti Aminah", NIK = "222", Phone = "082", CreatedAt = DateTime.Now };
        var sitiAcc = new Account 
        { 
            AccountId = 2, 
            CustomerId = 2, 
            AccountTypeId = 1, // Savings
            AccountNumber = "1000000002", 
            Balance = 150_000, // Just enough for 100k withdrawal (min bal 50k)
            DailyLimit = 10_000_000, 
            IsActive = true 
        };
        var sitiCard = new Card 
        { 
            CardId = 2, 
            CustomerId = 2, 
            AccountId = 2, 
            CardNumber = "5555666677778888", 
            PinHash = BCrypt.Net.BCrypt.HashPassword("654321"), 
            IsBlocked = false,
            ExpiryDate = DateTime.Now.AddYears(3)
        };

        // Andi: Rich / Fraud Test
        var andi = new Customer { CustomerId = 3, FullName = "Andi Sultan", NIK = "333", Phone = "083", CreatedAt = DateTime.Now };
        var andiAcc = new Account 
        { 
            AccountId = 3, 
            CustomerId = 3, 
            AccountTypeId = 3, // Business
            AccountNumber = "1000000003", 
            Balance = 100_000_000, 
            DailyLimit = 50_000_000, 
            IsActive = true 
        };
        var andiCard = new Card 
        { 
            CardId = 3, 
            CustomerId = 3, 
            AccountId = 3, 
            CardNumber = "9999888877776666", 
            PinHash = BCrypt.Net.BCrypt.HashPassword("112233"), 
            IsBlocked = false,
            ExpiryDate = DateTime.Now.AddYears(3)
        };

        _db.Customers.AddRange(budi, siti, andi);
        _db.Accounts.AddRange(budiAcc, sitiAcc, andiAcc);
        _db.Cards.AddRange(budiCard, sitiCard, andiCard);

        _db.SaveChanges();
    }

    public void Dispose()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    private AuthService CreateAuthService()
    {
        var configDict = new Dictionary<string, string?>
        {
            { "Jwt:Key", "BankingEcosystemSuperSecretKey2026!@#$%^&*()" },
            { "Jwt:Issuer", "BankingEcosystem" },
            { "Jwt:Audience", "BankingEcosystemClient" }
        };
        var config = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();
        return new AuthService(_db, config);
    }

    private string EncryptPin(string plainPin)
    {
        const string Key = "BANK_ECO_SECURE_2026";
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < plainPin.Length; i++)
        {
            char c = plainPin[i];
            char keyChar = Key[i % Key.Length];
            byte b = (byte)(c ^ keyChar);
            sb.Append(b.ToString("X2"));
        }
        return sb.ToString();
    }

    // ─── SCENARIOS ───

    [Fact]
    public async Task Scenario_NormalWithdrawal_Flow()
    {
        // 1. Authenticate (Login)
        var authService = CreateAuthService();
        var loginResult = await authService.VerifyPinAsync(1, EncryptPin("123456"));
        
        Assert.NotNull(loginResult);
        Assert.Equal("Budi Santoso", loginResult!.CustomerName);

        // 2. Check Balance
        var txService = new TransactionService(_db);
        var balance = await txService.BalanceInquiryAsync(1, 1);
        Assert.Equal(5_000_000, balance.BalanceAfter);

        // 3. Withdraw 100k
        var withdrawResult = await txService.WithdrawAsync(new WithdrawRequest(1, 1, 100_000));
        Assert.Equal("Success", withdrawResult.Status);
        Assert.Equal(4_900_000, withdrawResult.BalanceAfter);

        // 4. Verify DB State
        var account = await _db.Accounts.FindAsync(1);
        Assert.Equal(4_900_000, account!.Balance);
    }

    [Fact]
    public async Task Scenario_Transfer_Flow()
    {
        // 1. Budi Transfers 500k to Siti
        var txService = new TransactionService(_db);
        
        var transferResult = await txService.TransferAsync(new TransferRequest(1, "1000000002", 500_000, "Gift"));
        
        Assert.Equal("Success", transferResult.Status);
        Assert.Equal(4_500_000, transferResult.BalanceAfter); // 5M - 500k

        // 2. Check Siti's Balance
        var sitiAccount = await _db.Accounts.FindAsync(2);
        Assert.Equal(150_000 + 500_000, sitiAccount!.Balance); // 650k
    }

    [Fact]
    public async Task Scenario_LowBalance_Withdrawal_Fails()
    {
        // Siti has 150k. Min balance is 50k. Max withdrawable = 100k.
        var txService = new TransactionService(_db);

        // Try withdrawing 150k (would leave 0, violates min balance)
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
        {
            await txService.WithdrawAsync(new WithdrawRequest(2, 1, 150_000));
        });
    }

    [Fact]
    public async Task Scenario_Fraud_DailyLimit_Exceeded()
    {
        // Andi has 100M balance, 50M limit.
        var txService = new TransactionService(_db);

        // 1. Withdraw 50M (Hit Limit)
        var res1 = await txService.WithdrawAsync(new WithdrawRequest(3, 1, 50_000_000));
        Assert.Equal("Success", res1.Status);

        // 2. Try Withdraw another 10k (Exceed Limit)
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => 
        {
            await txService.WithdrawAsync(new WithdrawRequest(3, 1, 10_000));
        });

        Assert.Contains("Daily withdrawal limit exceeded", exception.Message);
    }
}
