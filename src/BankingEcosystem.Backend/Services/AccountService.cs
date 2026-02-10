using Microsoft.EntityFrameworkCore;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Backend.DTOs;
using BankingEcosystem.Backend.Models;

namespace BankingEcosystem.Backend.Services;

public class AccountService(BankingDbContext db)
{
    public async Task<AccountDto?> GetByIdAsync(int accountId)
    {
        var acc = await db.Accounts
            .Include(a => a.AccountType)
            .FirstOrDefaultAsync(a => a.AccountId == accountId);

        return acc == null ? null : MapToDto(acc);
    }

    public async Task<AccountDto?> GetByAccountNumberAsync(string accountNumber)
    {
        var acc = await db.Accounts
            .Include(a => a.AccountType)
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

        return acc == null ? null : MapToDto(acc);
    }

    public async Task<List<AccountDto>> GetByCustomerIdAsync(int customerId)
    {
        return await db.Accounts
            .Include(a => a.AccountType)
            .Where(a => a.CustomerId == customerId)
            .Select(a => MapToDto(a))
            .ToListAsync();
    }

    public async Task<AccountDto> CreateAsync(CreateAccountRequest request)
    {
        var accountType = await db.AccountTypes.FindAsync(request.AccountTypeId)
            ?? throw new ArgumentException("Invalid account type");

        var account = new Account
        {
            CustomerId = request.CustomerId,
            AccountTypeId = request.AccountTypeId,
            AccountNumber = GenerateAccountNumber(),
            Balance = request.InitialDeposit,
            DailyLimit = accountType.DefaultDailyLimit,
            IsActive = true
        };

        db.Accounts.Add(account);
        await db.SaveChangesAsync();

        return new AccountDto(account.AccountId, account.AccountNumber,
            accountType.TypeName, account.Balance, account.DailyLimit, account.IsActive, account.CreatedAt);
    }

    private static string GenerateAccountNumber()
    {
        return $"ACC{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(100000, 999999)}";
    }

    private static AccountDto MapToDto(Account a) =>
        new(a.AccountId, a.AccountNumber, a.AccountType.TypeName, a.Balance, a.DailyLimit, a.IsActive, a.CreatedAt);
}
