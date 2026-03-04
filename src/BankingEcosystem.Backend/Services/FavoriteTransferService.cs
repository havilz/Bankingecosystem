using Microsoft.EntityFrameworkCore;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Backend.Models;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Backend.Services;

public class FavoriteTransferService(BankingDbContext db)
{
    public async Task<List<FavoriteDto>> GetFavoritesAsync(int accountId)
    {
        return await db.FavoriteTransfers
            .Where(f => f.AccountId == accountId)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new FavoriteDto(f.FavoriteId, f.FavoriteAccountNumber, f.Nickname, f.BankName))
            .ToListAsync();
    }

    public async Task<FavoriteDto> AddFavoriteAsync(AddFavoriteRequest request)
    {
        var favorite = new FavoriteTransfer
        {
            AccountId = request.AccountId,
            FavoriteAccountNumber = request.AccountNumber,
            Nickname = request.Nickname,
            BankName = request.BankName,
            CreatedAt = DateTime.UtcNow
        };
        db.FavoriteTransfers.Add(favorite);
        await db.SaveChangesAsync();

        return new FavoriteDto(favorite.FavoriteId, favorite.FavoriteAccountNumber,
            favorite.Nickname, favorite.BankName);
    }

    public async Task<bool> RemoveFavoriteAsync(int favoriteId, int accountId)
    {
        var favorite = await db.FavoriteTransfers
            .FirstOrDefaultAsync(f => f.FavoriteId == favoriteId && f.AccountId == accountId);

        if (favorite == null) return false;

        db.FavoriteTransfers.Remove(favorite);
        await db.SaveChangesAsync();
        return true;
    }
}
