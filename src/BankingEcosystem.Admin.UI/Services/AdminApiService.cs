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
}
