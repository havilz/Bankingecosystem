using Microsoft.EntityFrameworkCore;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Backend.Services;

public class BankService(BankingDbContext db)
{
    public async Task<List<BankDto>> GetAllBanksAsync()
    {
        return await db.Banks
            .OrderBy(b => b.BankName)
            .Select(b => new BankDto(b.BankId, b.BankCode, b.BankName))
            .ToListAsync();
    }
}
