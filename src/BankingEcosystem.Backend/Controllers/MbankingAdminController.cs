using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankingEcosystem.Backend.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Backend.Controllers;

[ApiController]
[Route("api/admin/mbanking")]
public class MbankingAdminController(MbankingAdminService service) : ControllerBase
{
    // GET /api/admin/mbanking
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var accounts = await service.GetAllAsync();
        return Ok(new ApiResponse<List<MbankingAccountDto>>(true, "OK", accounts));
    }

    // PUT /api/admin/mbanking/email
    [HttpPut("email")]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateMbankingEmailRequest request)
    {
        try
        {
            var success = await service.UpdateEmailAsync(request.MbankingAccountId, request.NewEmail);
            if (!success)
                return NotFound(new ApiResponse<object>(false, "Akun mbanking tidak ditemukan.", null));
            return Ok(new ApiResponse<object>(true, "Email berhasil diperbarui.", null));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message, null));
        }
    }

    // DELETE /api/admin/mbanking/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await service.DeleteAsync(id);
        if (!success)
            return NotFound(new ApiResponse<object>(false, "Akun mbanking tidak ditemukan.", null));
        return Ok(new ApiResponse<object>(true, "Akun mbanking berhasil dihapus.", null));
    }
}
