using Microsoft.AspNetCore.Mvc;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Backend.Services;

namespace BankingEcosystem.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(AdminService adminService, AuditLogService auditLogService) : ControllerBase
{
    // ─── Customer ───
    [HttpGet("customers")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await adminService.GetAllCustomersAsync();
        return Ok(new ApiResponse<List<CustomerDto>>(true, "OK", customers));
    }

    [HttpGet("customers/{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await adminService.GetCustomerDetailAsync(id);
        if (customer == null) return NotFound(new ApiResponse<object>(false, "Customer not found", null));
        return Ok(new ApiResponse<CustomerDetailDto>(true, "OK", customer));
    }

    [HttpPost("customers")]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        try
        {
            var customer = await adminService.CreateCustomerAsync(request);
            await auditLogService.LogAsync(null, "CREATE_CUSTOMER", "Customer", customer.CustomerId, $"Created customer: {customer.FullName}");
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, new ApiResponse<CustomerDto>(true, "Customer created", customer));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message, null));
        }
    }

    [HttpPut("customers/{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerRequest request)
    {
        var result = await adminService.UpdateCustomerAsync(id, request);
        if (!result) return NotFound(new ApiResponse<object>(false, "Customer not found", null));
        await auditLogService.LogAsync(null, "UPDATE_CUSTOMER", "Customer", id, $"Updated customer: {request.FullName}");
        return Ok(new ApiResponse<object>(true, "Customer updated", null));
    }

    // ─── Account ───
    [HttpGet("customers/{id}/accounts")]
    public async Task<IActionResult> GetAccountsByCustomer(int id)
    {
        var accounts = await adminService.GetAccountsByCustomerAsync(id);
        return Ok(new ApiResponse<List<AccountDto>>(true, "OK", accounts));
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        try
        {
            var account = await adminService.CreateAccountAsync(request);
            await auditLogService.LogAsync(null, "CREATE_ACCOUNT", "Account", account.AccountId, $"Created account: {account.AccountNumber}");
            return Ok(new ApiResponse<AccountDto>(true, "Account created", account));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message, null));
        }
    }

    // ─── Card ───
    [HttpGet("customers/{id}/cards")]
    public async Task<IActionResult> GetCardsByCustomer(int id)
    {
        var cards = await adminService.GetCardsByCustomerAsync(id);
        return Ok(new ApiResponse<List<CardDto>>(true, "OK", cards));
    }

    [HttpPost("cards")]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
    {
        try
        {
            var card = await adminService.CreateCardAsync(request);
            await auditLogService.LogAsync(null, "CREATE_CARD", "Card", card.CardId, $"Issued card: {card.CardNumber}");
            return Ok(new ApiResponse<CardDto>(true, "Card issued", card));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(false, ex.Message, null));
        }
    }

    // ─── Account Types ───
    [HttpGet("account-types")]
    public async Task<IActionResult> GetAccountTypes()
    {
        var types = await adminService.GetAccountTypesAsync();
        return Ok(new ApiResponse<List<AccountTypeDto>>(true, "OK", types));
    }

    // ─── ATM ───
    [HttpGet("atms")]
    public async Task<IActionResult> GetAllAtms()
    {
        var atms = await adminService.GetAllAtmsAsync();
        return Ok(new ApiResponse<List<AtmDto>>(true, "OK", atms));
    }

    [HttpPost("atms")]
    public async Task<IActionResult> CreateAtm([FromBody] CreateAtmRequest request)
    {
        var atm = await adminService.CreateAtmAsync(request);
        await auditLogService.LogAsync(null, "CREATE_ATM", "Atm", atm.AtmId, $"Created ATM: {atm.AtmCode} at {atm.Location}");
        return Ok(new ApiResponse<AtmDto>(true, "ATM created", atm));
    }

    [HttpPost("atms/refill")]
    public async Task<IActionResult> RefillAtm([FromBody] RefillAtmRequest request)
    {
        var result = await adminService.RefillAtmAsync(request);
        if (!result) return NotFound(new ApiResponse<object>(false, "ATM not found", null));
        await auditLogService.LogAsync(null, "REFILL_ATM", "Atm", request.AtmId, $"Refilled: {request.Quantity}x Rp{request.Denomination:N0}");
        return Ok(new ApiResponse<object>(true, "ATM refilled", null));
    }

    [HttpPatch("atms/{id}/toggle")]
    public async Task<IActionResult> ToggleAtm(int id)
    {
        var result = await adminService.ToggleAtmStatusAsync(id);
        if (!result) return NotFound(new ApiResponse<object>(false, "ATM not found", null));
        await auditLogService.LogAsync(null, "TOGGLE_ATM", "Atm", id, "Toggled online/offline status");
        return Ok(new ApiResponse<object>(true, "ATM status toggled", null));
    }

    // ─── Reporting ───
    [HttpGet("reports/dashboard")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var stats = await adminService.GetDashboardStatsAsync();
        return Ok(new ApiResponse<DashboardStatsDto>(true, "OK", stats));
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 50, [FromQuery] string? search = null, [FromQuery] string? type = null)
    {
        var result = await adminService.GetTransactionsAsync(page, pageSize, search, type);
        return Ok(new ApiResponse<PaginatedResponse<TransactionDto>>(true, "OK", result));
    }

    // ─── Employee ───
    [HttpGet("employees")]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await adminService.GetAllEmployeesAsync();
        return Ok(new ApiResponse<List<EmployeeDto>>(true, "OK", employees));
    }

    [HttpPost("employees")]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
    {
        var emp = await adminService.CreateEmployeeAsync(request);
        await auditLogService.LogAsync(null, "CREATE_EMPLOYEE", "Employee", emp.EmployeeId, $"Created employee: {emp.FullName} ({emp.Role})");
        return Ok(new ApiResponse<EmployeeDto>(true, "Employee created", emp));
    }

    // ─── Audit Log ───
    [HttpGet("audit-logs")]
    public async Task<IActionResult> GetAuditLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var logs = await auditLogService.GetLogsAsync(page, pageSize);
        return Ok(new ApiResponse<List<AuditLogDto>>(true, "OK", logs));
    }
}
