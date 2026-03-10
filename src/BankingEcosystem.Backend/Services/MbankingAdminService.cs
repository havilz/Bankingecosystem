using Microsoft.EntityFrameworkCore;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Backend.Services;

public class MbankingAdminService(BankingDbContext db)
{
    // GET all registered mbanking accounts with customer/card info
    public async Task<List<MbankingAccountDto>> GetAllAsync()
    {
        return await db.MbankingAccounts
            .Include(m => m.Card)
                .ThenInclude(c => c.Account)
                .ThenInclude(a => a.Customer)
            .OrderByDescending(m => m.RegisteredAt)
            .Select(m => new MbankingAccountDto(
                m.MbankingAccountId,
                m.CardId,
                m.Card.CardNumber,
                m.Card.Account.Customer.FullName,
                m.Card.Account.AccountNumber,
                m.Email,
                m.RegisteredAt
            ))
            .ToListAsync();
    }

    // UPDATE email for a mbanking account
    public async Task<bool> UpdateEmailAsync(int mbankingAccountId, string newEmail)
    {
        var existing = await db.MbankingAccounts.FindAsync(mbankingAccountId);
        if (existing is null) return false;

        // Check for email conflict
        var emailTaken = await db.MbankingAccounts
            .AnyAsync(m => m.Email == newEmail && m.MbankingAccountId != mbankingAccountId);
        if (emailTaken)
            throw new InvalidOperationException("Email sudah digunakan oleh akun lain.");

        existing.Email = newEmail;
        await db.SaveChangesAsync();
        return true;
    }

    // DELETE a mbanking account (revoke access, does NOT delete customer or card)
    public async Task<bool> DeleteAsync(int mbankingAccountId)
    {
        var existing = await db.MbankingAccounts.FindAsync(mbankingAccountId);
        if (existing is null) return false;

        db.MbankingAccounts.Remove(existing);
        await db.SaveChangesAsync();
        return true;
    }
}
