namespace BankingEcosystem.Backend.Models;

public class Card
{
    public int CardId { get; set; }
    public int CustomerId { get; set; }
    public int AccountId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string PinHash { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsBlocked { get; set; } = false;
    public int FailedAttempts { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Customer Customer { get; set; } = null!;
    public Account Account { get; set; } = null!;
}
