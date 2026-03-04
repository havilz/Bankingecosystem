using Microsoft.AspNetCore.Mvc;
using BankingEcosystem.Backend.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferController(FavoriteTransferService favoriteService) : ControllerBase
{
    // GET /api/Transfer/favorites/{accountId}
    [HttpGet("favorites/{accountId:int}")]
    public async Task<IActionResult> GetFavorites(int accountId)
    {
        var favorites = await favoriteService.GetFavoritesAsync(accountId);
        return Ok(new ApiResponse<List<FavoriteDto>>(true, "OK", favorites));
    }

    // POST /api/Transfer/favorites
    [HttpPost("favorites")]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequest request)
    {
        var result = await favoriteService.AddFavoriteAsync(request);
        return Ok(new ApiResponse<FavoriteDto>(true, "Favorit berhasil ditambahkan.", result));
    }

    // DELETE /api/Transfer/favorites/{id}?accountId={accountId}
    [HttpDelete("favorites/{id:int}")]
    public async Task<IActionResult> RemoveFavorite(int id, [FromQuery] int accountId)
    {
        var success = await favoriteService.RemoveFavoriteAsync(id, accountId);
        if (!success)
            return NotFound(new ApiResponse<object>(false, "Favorit tidak ditemukan.", null));

        return Ok(new ApiResponse<object>(true, "Favorit dihapus.", null));
    }
}
