using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Tests;

/// <summary>
/// Tests for DTO records — ensuring correct construction and value integrity.
/// DTOs are the contract between Client and Backend.
/// </summary>
public class DtoTests
{
    // ─── Auth DTOs ───

    [Fact]
    public void VerifyCardRequest_CreatesCorrectly()
    {
        var request = new VerifyCardRequest("6221453754749809");
        Assert.Equal("6221453754749809", request.CardNumber);
    }

    [Fact]
    public void VerifyCardResponse_IncludesAllFields()
    {
        var response = new VerifyCardResponse(1, 1, "1234567890", "John Doe", false);
        Assert.Equal(1, response.CardId);
        Assert.Equal(1, response.AccountId);
        Assert.Equal("1234567890", response.AccountNumber);
        Assert.Equal("John Doe", response.CustomerName);
        Assert.False(response.IsBlocked);
    }

    [Fact]
    public void VerifyPinRequest_CreatesCorrectly()
    {
        var request = new VerifyPinRequest(1, "123456");
        Assert.Equal(1, request.CardId);
        Assert.Equal("123456", request.Pin);
    }

    [Fact]
    public void AuthResponse_IncludesAccountId()
    {
        var response = new AuthResponse("jwt-token", "1234567890", "John Doe", 5000000m, 1);
        Assert.Equal("jwt-token", response.Token);
        Assert.Equal("1234567890", response.AccountNumber);
        Assert.Equal("John Doe", response.CustomerName);
        Assert.Equal(5000000m, response.Balance);
        Assert.Equal(1, response.AccountId);
    }

    // ─── Transaction DTOs ───

    [Fact]
    public void WithdrawRequest_CreatesCorrectly()
    {
        var request = new WithdrawRequest(1, 1, 500000m);
        Assert.Equal(1, request.AccountId);
        Assert.Equal(1, request.AtmId);
        Assert.Equal(500000m, request.Amount);
    }

    [Fact]
    public void DepositRequest_CreatesCorrectly()
    {
        var request = new DepositRequest(1, 1000000m);
        Assert.Equal(1, request.AccountId);
        Assert.Equal(1000000m, request.Amount);
    }

    [Fact]
    public void TransferRequest_CreatesCorrectly()
    {
        var request = new TransferRequest(1, "9876543210", 250000m, "Test transfer");
        Assert.Equal(1, request.AccountId);
        Assert.Equal("9876543210", request.TargetAccountNumber);
        Assert.Equal(250000m, request.Amount);
        Assert.Equal("Test transfer", request.Description);
    }

    [Fact]
    public void TransferRequest_DescriptionCanBeNull()
    {
        var request = new TransferRequest(1, "9876543210", 250000m, null);
        Assert.Null(request.Description);
    }

    // ─── ApiResponse Wrapper ───

    [Fact]
    public void ApiResponse_SuccessWrapper()
    {
        var response = new ApiResponse<string>(true, "OK", "data");
        Assert.True(response.Success);
        Assert.Equal("OK", response.Message);
        Assert.Equal("data", response.Data);
    }

    [Fact]
    public void ApiResponse_FailureWrapper()
    {
        var response = new ApiResponse<string>(false, "Error occurred", null);
        Assert.False(response.Success);
        Assert.Equal("Error occurred", response.Message);
        Assert.Null(response.Data);
    }

    // ─── Admin DTOs ───

    [Fact]
    public void CreateAtmRequest_CreatesCorrectly()
    {
        var request = new CreateAtmRequest("ATM001", "Jakarta Pusat", 50000000m);
        Assert.Equal("ATM001", request.AtmCode);
        Assert.Equal("Jakarta Pusat", request.Location);
        Assert.Equal(50000000m, request.InitialCash);
    }
}
