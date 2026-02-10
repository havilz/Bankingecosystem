namespace BankingEcosystem.Backend.Models;

public class Transaction
{
    public int TransactionId { get; set; }
    public int AccountId { get; set; }
    public int? AtmId { get; set; }
    public int TransactionTypeId { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string? TargetAccountNumber { get; set; }
    public string Status { get; set; } = "Pending";
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Account Account { get; set; } = null!;
    public Atm? Atm { get; set; }
    public TransactionType TransactionType { get; set; } = null!;
}
