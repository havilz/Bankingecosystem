using Microsoft.AspNetCore.Mvc;
using BankingEcosystem.Backend.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankController(BankService bankService) : ControllerBase
{
    // GET /api/Bank
    [HttpGet]
    public async Task<IActionResult> GetBanks()
    {
        var banks = await bankService.GetAllBanksAsync();
        return Ok(new ApiResponse<List<BankDto>>(true, "OK", banks));
    }
}
