using Microsoft.EntityFrameworkCore;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Backend.Models;

namespace BankingEcosystem.Backend.Services;

public class CardService(BankingDbContext db)
{
    public async Task<CardDto?> GetByIdAsync(int cardId)
    {
        var card = await db.Cards
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.CardId == cardId);

        return card == null ? null : MapToDto(card);
    }

    public async Task<List<CardDto>> GetByCustomerIdAsync(int customerId)
    {
        return await db.Cards
            .Include(c => c.Account)
            .Where(c => c.CustomerId == customerId)
            .Select(c => MapToDto(c))
            .ToListAsync();
    }

    public async Task<CardDto> CreateAsync(CreateCardRequest request)
    {
        var card = new Card
        {
            CustomerId = request.CustomerId,
            AccountId = request.AccountId,
            CardNumber = GenerateCardNumber(),
            PinHash = BCrypt.Net.BCrypt.HashPassword(request.Pin),
            ExpiryDate = DateTime.UtcNow.AddYears(5),
            IsBlocked = false,
            FailedAttempts = 0
        };

        db.Cards.Add(card);
        await db.SaveChangesAsync();

        var account = await db.Accounts.FindAsync(request.AccountId);
        return new CardDto(card.CardId, card.CardNumber, card.AccountId,
            account?.AccountNumber ?? "", card.ExpiryDate, card.IsBlocked, card.FailedAttempts);
    }

    public async Task<bool> BlockCardAsync(int cardId)
    {
        var card = await db.Cards.FindAsync(cardId);
        if (card == null) return false;
        card.IsBlocked = true;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnblockCardAsync(int cardId)
    {
        var card = await db.Cards.FindAsync(cardId);
        if (card == null) return false;
        card.IsBlocked = false;
        card.FailedAttempts = 0;
        await db.SaveChangesAsync();
        return true;
    }

    private static string GenerateCardNumber()
    {
        return $"6221{Random.Shared.NextInt64(100000000000, 999999999999)}";
    }

    private static CardDto MapToDto(Card c) =>
        new(c.CardId, c.CardNumber, c.AccountId, c.Account.AccountNumber, c.ExpiryDate, c.IsBlocked, c.FailedAttempts);
}
