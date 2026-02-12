namespace BankingEcosystem.Shared.DTOs;

// ─── Auth ───
public record VerifyCardRequest(string CardNumber);
public record VerifyCardResponse(int CardId, int AccountId, string AccountNumber, string CustomerName, bool IsBlocked);
public record VerifyPinRequest(int CardId, string Pin);
public record AuthResponse(string Token, string AccountNumber, string CustomerName, decimal Balance, int AccountId);

public record EmployeeLoginRequest(string EmployeeCode, string Password);
public record EmployeeLoginResponse(string Token, string FullName, string Role);

// ─── Account ───
public record AccountDto(int AccountId, string AccountNumber, string AccountTypeName, decimal Balance, decimal DailyLimit, bool IsActive, DateTime CreatedAt);
public record CreateAccountRequest(int CustomerId, int AccountTypeId, decimal InitialDeposit);

// ─── Customer ───
public record CustomerDto(int CustomerId, string FullName, string NIK, string Phone, string? Email, string? Address, DateTime DateOfBirth, DateTime CreatedAt);
public record CreateCustomerRequest(string FullName, string NIK, string Phone, string? Email, string? Address, DateTime DateOfBirth);
public record UpdateCustomerRequest(string FullName, string Phone, string? Email, string? Address);
public record CustomerDetailDto(int CustomerId, string FullName, string NIK, string Phone, string? Email, string? Address, DateTime DateOfBirth, DateTime CreatedAt, List<AccountDto> Accounts, List<CardDto> Cards);

// ─── AccountType ───
public record AccountTypeDto(int AccountTypeId, string TypeName, decimal MinBalance, decimal DefaultDailyLimit);

// ─── Card ───
public record CardDto(int CardId, string CardNumber, int AccountId, string AccountNumber, DateTime ExpiryDate, bool IsBlocked, int FailedAttempts);
public record CreateCardRequest(int CustomerId, int AccountId, string Pin);

// ─── Transaction ───
// ─── Transaction ───
public record TransactionDto(int TransactionId, string TransactionType, decimal Amount, decimal BalanceBefore, decimal BalanceAfter, string ReferenceNumber, string? TargetAccountNumber, string Status, string? Description, DateTime CreatedAt, string? AtmCode, string? Location);
public record WithdrawRequest(int AccountId, int AtmId, decimal Amount);
public record DepositRequest(int AccountId, decimal Amount);
public record TransferRequest(int AccountId, string TargetAccountNumber, decimal Amount, string? Description);

// ─── ATM ───
public record AtmDto(int AtmId, string AtmCode, string Location, bool IsOnline, decimal TotalCash, DateTime? LastRefill);
public record CreateAtmRequest(string AtmCode, string Location, decimal InitialCash);
public record RefillAtmRequest(int AtmId, int Denomination, int Quantity);
public record DashboardStatsDto(int TotalCustomers, int TotalAtms, int TotalTransactionsToday, decimal TotalBalanceStored);

// ─── Employee ───
public record EmployeeDto(int EmployeeId, string FullName, string EmployeeCode, string Role, bool IsActive, DateTime CreatedAt);
public record CreateEmployeeRequest(string FullName, string EmployeeCode, string Password, string Role);

// ─── Audit Log ───
public record AuditLogDto(int LogId, int? EmployeeId, string? EmployeeName, string Action, string EntityType, int? EntityId, string? Details, DateTime CreatedAt);

// ─── Generic ───
public record ApiResponse<T>(bool Success, string Message, T? Data);
public record PaginatedResponse<T>(IEnumerable<T> Items, int TotalCount, int PageNum, int PageSize);
