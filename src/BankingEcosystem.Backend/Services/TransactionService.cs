using Microsoft.EntityFrameworkCore;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Backend.DTOs;
using BankingEcosystem.Backend.Models;

namespace BankingEcosystem.Backend.Services;

public class TransactionService(BankingDbContext db)
{
    public async Task<TransactionDto> WithdrawAsync(WithdrawRequest request)
    {
        var account = await db.Accounts.FindAsync(request.AccountId)
            ?? throw new ArgumentException("Account not found");

        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be positive");

        if (request.Amount > account.Balance - 50_000m)
            throw new InvalidOperationException("Insufficient balance (minimum Rp 50.000 must remain)");

        if (request.Amount > account.DailyLimit)
            throw new InvalidOperationException($"Amount exceeds daily limit of Rp {account.DailyLimit:N0}");

        var balanceBefore = account.Balance;
        account.Balance -= request.Amount;

        var tx = new Transaction
        {
            AccountId = request.AccountId,
            AtmId = request.AtmId,
            TransactionTypeId = 1, // Withdrawal
            Amount = request.Amount,
            BalanceBefore = balanceBefore,
            BalanceAfter = account.Balance,
            ReferenceNumber = GenerateRefNumber(),
            Status = "Success"
        };

        db.Transactions.Add(tx);
        await db.SaveChangesAsync();
        return MapToDto(tx);
    }

    public async Task<TransactionDto> DepositAsync(DepositRequest request)
    {
        var account = await db.Accounts.FindAsync(request.AccountId)
            ?? throw new ArgumentException("Account not found");

        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be positive");

        var balanceBefore = account.Balance;
        account.Balance += request.Amount;

        var tx = new Transaction
        {
            AccountId = request.AccountId,
            TransactionTypeId = 2, // Deposit
            Amount = request.Amount,
            BalanceBefore = balanceBefore,
            BalanceAfter = account.Balance,
            ReferenceNumber = GenerateRefNumber(),
            Status = "Success"
        };

        db.Transactions.Add(tx);
        await db.SaveChangesAsync();
        return MapToDto(tx);
    }

    public async Task<TransactionDto> TransferAsync(TransferRequest request)
    {
        var source = await db.Accounts.FindAsync(request.AccountId)
            ?? throw new ArgumentException("Source account not found");

        var target = await db.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.TargetAccountNumber)
            ?? throw new ArgumentException("Target account not found");

        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be positive");

        if (request.Amount > source.Balance - 50_000m)
            throw new InvalidOperationException("Insufficient balance");

        if (request.Amount > 100_000_000m)
            throw new InvalidOperationException("Transfer exceeds maximum limit of Rp 100.000.000");

        var balanceBefore = source.Balance;
        source.Balance -= request.Amount;
        target.Balance += request.Amount;

        var tx = new Transaction
        {
            AccountId = request.AccountId,
            TransactionTypeId = 3, // Transfer
            Amount = request.Amount,
            BalanceBefore = balanceBefore,
            BalanceAfter = source.Balance,
            ReferenceNumber = GenerateRefNumber(),
            TargetAccountNumber = request.TargetAccountNumber,
            Status = "Success",
            Description = request.Description
        };

        db.Transactions.Add(tx);
        await db.SaveChangesAsync();
        return MapToDto(tx);
    }

    public async Task<TransactionDto> BalanceInquiryAsync(int accountId, int? atmId)
    {
        var account = await db.Accounts.FindAsync(accountId)
            ?? throw new ArgumentException("Account not found");

        var tx = new Transaction
        {
            AccountId = accountId,
            AtmId = atmId,
            TransactionTypeId = 4, // BalanceInquiry
            Amount = 0,
            BalanceBefore = account.Balance,
            BalanceAfter = account.Balance,
            ReferenceNumber = GenerateRefNumber(),
            Status = "Success"
        };

        db.Transactions.Add(tx);
        await db.SaveChangesAsync();
        return MapToDto(tx);
    }

    public async Task<List<TransactionDto>> GetHistoryAsync(int accountId, int page = 1, int pageSize = 20)
    {
        return await db.Transactions
            .Include(t => t.TransactionType)
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => MapToDto(t))
            .ToListAsync();
    }

    private static string GenerateRefNumber()
    {
        return $"TXN{DateTime.UtcNow:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
    }

    private static TransactionDto MapToDto(Transaction t) =>
        new(t.TransactionId, t.TransactionType?.TypeName ?? "Unknown", t.Amount,
            t.BalanceBefore, t.BalanceAfter, t.ReferenceNumber,
            t.TargetAccountNumber, t.Status, t.Description, t.CreatedAt);
}
