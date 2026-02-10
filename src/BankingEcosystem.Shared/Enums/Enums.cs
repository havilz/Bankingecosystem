namespace BankingEcosystem.Shared.Enums;

public enum TransactionType
{
    Withdrawal = 1,
    Deposit = 2,
    Transfer = 3,
    BalanceInquiry = 4
}

public enum AccountType
{
    Savings = 1,
    Checking = 2,
    Business = 3
}

public enum AtmState
{
    Idle = 0,
    CardInserted = 1,
    PinEntry = 2,
    Authenticated = 3,
    Transaction = 4,
    Dispensing = 5,
    Completed = 6,
    Error = 7
}

public enum TransactionStatus
{
    Pending,
    Success,
    Failed,
    Cancelled
}
