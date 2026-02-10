using Microsoft.AspNetCore.Mvc;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Backend.Services;

namespace BankingEcosystem.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(AccountService accountService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var account = await accountService.GetByIdAsync(id);
        if (account == null) return NotFound(new ApiResponse<object>(false, "Account not found", null));
        return Ok(new ApiResponse<AccountDto>(true, "OK", account));
    }

    [HttpGet("by-number/{accountNumber}")]
    public async Task<IActionResult> GetByNumber(string accountNumber)
    {
        var account = await accountService.GetByAccountNumberAsync(accountNumber);
        if (account == null) return NotFound(new ApiResponse<object>(false, "Account not found", null));
        return Ok(new ApiResponse<AccountDto>(true, "OK", account));
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomer(int customerId)
    {
        var accounts = await accountService.GetByCustomerIdAsync(customerId);
        return Ok(new ApiResponse<List<AccountDto>>(true, "OK", accounts));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        try
        {
            var account = await accountService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = account.AccountId }, new ApiResponse<AccountDto>(true, "Account created", account));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message, null));
        }
    }
}
