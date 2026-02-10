using Microsoft.AspNetCore.Mvc;
using BankingEcosystem.Backend.DTOs;
using BankingEcosystem.Backend.Services;

namespace BankingEcosystem.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(TransactionService transactionService) : ControllerBase
{
    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
    {
        try
        {
            var result = await transactionService.WithdrawAsync(request);
            return Ok(new ApiResponse<TransactionDto>(true, "Withdrawal successful", result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message, null));
        }
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
    {
        try
        {
            var result = await transactionService.DepositAsync(request);
            return Ok(new ApiResponse<TransactionDto>(true, "Deposit successful", result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message, null));
        }
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
    {
        try
        {
            var result = await transactionService.TransferAsync(request);
            return Ok(new ApiResponse<TransactionDto>(true, "Transfer successful", result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message, null));
        }
    }

    [HttpGet("balance/{accountId}")]
    public async Task<IActionResult> BalanceInquiry(int accountId, [FromQuery] int? atmId)
    {
        try
        {
            var result = await transactionService.BalanceInquiryAsync(accountId, atmId);
            return Ok(new ApiResponse<TransactionDto>(true, "OK", result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message, null));
        }
    }

    [HttpGet("history/{accountId}")]
    public async Task<IActionResult> GetHistory(int accountId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var results = await transactionService.GetHistoryAsync(accountId, page, pageSize);
        return Ok(new ApiResponse<List<TransactionDto>>(true, "OK", results));
    }
}
