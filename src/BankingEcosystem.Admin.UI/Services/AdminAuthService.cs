using System.Net.Http;
using System.Net.Http.Json;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Admin.UI.Services;

/// <summary>
/// Handles employee authentication with the Backend API.
/// </summary>
public class AdminAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public string? AuthToken { get; private set; }
    public string? EmployeeName { get; private set; }
    public string? EmployeeRole { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(AuthToken);

    public AdminAuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Authenticate an employee via the backend API.
    /// Returns null on success, or an error message on failure.
    /// </summary>
    public async Task<string?> LoginAsync(string employeeCode, string password)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AdminApi");
            var request = new EmployeeLoginRequest(employeeCode, password);
            var response = await client.PostAsJsonAsync("api/auth/employee-login", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                return error?.Message ?? "Login gagal. Periksa kembali kredensial Anda.";
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeLoginResponse>>();

            if (result?.Success == true && result.Data != null)
            {
                AuthToken = result.Data.Token;
                EmployeeName = result.Data.FullName;
                EmployeeRole = result.Data.Role;
                return null; // Success
            }

            return result?.Message ?? "Login gagal.";
        }
        catch (HttpRequestException)
        {
            return "Tidak dapat terhubung ke server. Pastikan Backend API berjalan.";
        }
        catch (Exception ex)
        {
            return $"Terjadi kesalahan: {ex.Message}";
        }
    }

    public void Logout()
    {
        AuthToken = null;
        EmployeeName = null;
        EmployeeRole = null;
    }
}
