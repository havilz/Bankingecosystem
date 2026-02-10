using Microsoft.AspNetCore.Mvc;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Backend.Services;

namespace BankingEcosystem.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("verify-card")]
    public async Task<IActionResult> VerifyCard([FromBody] VerifyCardRequest request)
    {
        var result = await authService.VerifyCardAsync(request.CardNumber);
        if (result == null)
            return NotFound(new ApiResponse<object>(false, "Card not found", null));

        if (result.IsBlocked)
            return BadRequest(new ApiResponse<object>(false, "Card is blocked. Please contact your bank.", null));

        return Ok(new ApiResponse<VerifyCardResponse>(true, "Card verified", result));
    }

    [HttpPost("verify-pin")]
    public async Task<IActionResult> VerifyPin([FromBody] VerifyPinRequest request)
    {
        var result = await authService.VerifyPinAsync(request.CardId, request.Pin);
        if (result == null)
            return Unauthorized(new ApiResponse<object>(false, "Invalid PIN or card blocked", null));

        return Ok(new ApiResponse<AuthResponse>(true, "Authenticated", result));
    }

    [HttpPost("employee-login")]
    public async Task<IActionResult> EmployeeLogin([FromBody] EmployeeLoginRequest request)
    {
        var result = await authService.EmployeeLoginAsync(request.EmployeeCode, request.Password);
        if (result == null)
            return Unauthorized(new ApiResponse<object>(false, "Invalid credentials", null));

        return Ok(new ApiResponse<EmployeeLoginResponse>(true, "Login successful", result));
    }
}
