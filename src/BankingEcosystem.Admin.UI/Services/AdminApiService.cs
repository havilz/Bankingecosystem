using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Admin.UI.Services;

/// <summary>
/// HTTP client wrapper for Admin API endpoints.
/// </summary>
public class AdminApiService(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    // ─── Customers ───
    public async Task<List<CustomerDto>> GetCustomersAsync()
    {
        var resp = await httpClient.GetFromJsonAsync<ApiResponse<List<CustomerDto>>>("api/admin/customers", JsonOpts);
        return resp?.Data ?? [];
    }

    public async Task<CustomerDetailDto?> GetCustomerAsync(int id)
    {
        var resp = await httpClient.GetFromJsonAsync<ApiResponse<CustomerDetailDto>>($"api/admin/customers/{id}", JsonOpts);
        return resp?.Data;
    }

    public async Task<CustomerDto?> CreateCustomerAsync(CreateCustomerRequest request)
    {
        var resp = await httpClient.PostAsJsonAsync("api/admin/customers", request);
        var body = await resp.Content.ReadFromJsonAsync<ApiResponse<CustomerDto>>(JsonOpts);
        if (body is { Success: true }) return body.Data;
        throw new Exception(body?.Message ?? "Failed to create customer");
    }

    // ─── Accounts ───
    public async Task<List<AccountDto>> GetAccountsAsync(int customerId)
    {
        var resp = await httpClient.GetFromJsonAsync<ApiResponse<List<AccountDto>>>($"api/admin/customers/{customerId}/accounts", JsonOpts);
        return resp?.Data ?? [];
    }

    public async Task<AccountDto?> CreateAccountAsync(CreateAccountRequest request)
    {
        var resp = await httpClient.PostAsJsonAsync("api/admin/accounts", request);
        var body = await resp.Content.ReadFromJsonAsync<ApiResponse<AccountDto>>(JsonOpts);
        if (body is { Success: true }) return body.Data;
        throw new Exception(body?.Message ?? "Failed to create account");
    }

    // ─── Cards ───
    public async Task<List<CardDto>> GetCardsAsync(int customerId)
    {
        var resp = await httpClient.GetFromJsonAsync<ApiResponse<List<CardDto>>>($"api/admin/customers/{customerId}/cards", JsonOpts);
        return resp?.Data ?? [];
    }

    public async Task<CardDto?> CreateCardAsync(CreateCardRequest request)
    {
        var resp = await httpClient.PostAsJsonAsync("api/admin/cards", request);
        var body = await resp.Content.ReadFromJsonAsync<ApiResponse<CardDto>>(JsonOpts);
        if (body is { Success: true }) return body.Data;
        throw new Exception(body?.Message ?? "Failed to issue card");
    }

    // ─── Account Types ───
    public async Task<List<AccountTypeDto>> GetAccountTypesAsync()
    {
        var resp = await httpClient.GetFromJsonAsync<ApiResponse<List<AccountTypeDto>>>("api/admin/account-types", JsonOpts);
        return resp?.Data ?? [];
    }

    // ─── ATM Management ───
    public async Task<List<AtmDto>> GetAtmsAsync()
    {
        var resp = await httpClient.GetFromJsonAsync<ApiResponse<List<AtmDto>>>("api/admin/atms", JsonOpts);
        return resp?.Data ?? [];
    }

    public async Task<AtmDto?> CreateAtmAsync(CreateAtmRequest request)
    {
        var resp = await httpClient.PostAsJsonAsync("api/admin/atms", request);
        var body = await resp.Content.ReadFromJsonAsync<ApiResponse<AtmDto>>(JsonOpts);
        if (body is { Success: true }) return body.Data;
        throw new Exception(body?.Message ?? "Failed to create ATM");
    }

    public async Task RefillAtmAsync(RefillAtmRequest request)
    {
        var resp = await httpClient.PostAsJsonAsync("api/admin/atms/refill", request);
        if (!resp.IsSuccessStatusCode)
        {
             var body = await resp.Content.ReadFromJsonAsync<ApiResponse<object>>(JsonOpts);
             throw new Exception(body?.Message ?? "Failed to refill ATM");
        }
    }

    public async Task ToggleAtmStatusAsync(int atmId)
    {
        var resp = await httpClient.PatchAsync($"api/admin/atms/{atmId}/toggle", null);
         if (!resp.IsSuccessStatusCode)
        {
             var body = await resp.Content.ReadFromJsonAsync<ApiResponse<object>>(JsonOpts);
             throw new Exception(body?.Message ?? "Failed to toggle ATM status");
        }
    }

    // ─── Reporting ───
    public async Task<PaginatedResponse<TransactionDto>?> GetTransactionsAsync(int page = 1, int pageSize = 50, string? search = null, string? type = null)
    {
        var query = new List<string> { $"page={page}", $"pageSize={pageSize}" };
        if (!string.IsNullOrEmpty(search)) query.Add($"search={System.Net.WebUtility.UrlEncode(search)}");
        if (!string.IsNullOrEmpty(type)) query.Add($"type={System.Net.WebUtility.UrlEncode(type)}");
        
        var url = $"api/admin/transactions?{string.Join("&", query)}";
        var resp = await httpClient.GetFromJsonAsync<ApiResponse<PaginatedResponse<TransactionDto>>>(url, JsonOpts);
        return resp?.Data;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        var resp = await httpClient.GetFromJsonAsync<ApiResponse<DashboardStatsDto>>("api/admin/reports/dashboard", JsonOpts);
        return resp?.Data ?? new DashboardStatsDto(0, 0, 0, 0);
    }

    public async Task<List<AuditLogDto>> GetAuditLogsAsync(int page = 1, int pageSize = 50)
    {
        var url = $"api/admin/audit-logs?page={page}&pageSize={pageSize}";
        var resp = await httpClient.GetFromJsonAsync<ApiResponse<List<AuditLogDto>>>(url, JsonOpts);
        return resp?.Data ?? [];
    }
}
