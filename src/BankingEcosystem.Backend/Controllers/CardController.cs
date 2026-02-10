using Microsoft.AspNetCore.Mvc;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Backend.Services;

namespace BankingEcosystem.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardController(CardService cardService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var card = await cardService.GetByIdAsync(id);
        if (card == null) return NotFound(new ApiResponse<object>(false, "Card not found", null));
        return Ok(new ApiResponse<CardDto>(true, "OK", card));
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomer(int customerId)
    {
        var cards = await cardService.GetByCustomerIdAsync(customerId);
        return Ok(new ApiResponse<List<CardDto>>(true, "OK", cards));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCardRequest request)
    {
        var card = await cardService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = card.CardId }, new ApiResponse<CardDto>(true, "Card created", card));
    }

    [HttpPatch("{id}/block")]
    public async Task<IActionResult> Block(int id)
    {
        var result = await cardService.BlockCardAsync(id);
        if (!result) return NotFound(new ApiResponse<object>(false, "Card not found", null));
        return Ok(new ApiResponse<object>(true, "Card blocked", null));
    }

    [HttpPatch("{id}/unblock")]
    public async Task<IActionResult> Unblock(int id)
    {
        var result = await cardService.UnblockCardAsync(id);
        if (!result) return NotFound(new ApiResponse<object>(false, "Card not found", null));
        return Ok(new ApiResponse<object>(true, "Card unblocked", null));
    }
}
