namespace BankingEcosystem.Backend.Models;

public class MbankingAccount
{
    public int MbankingAccountId { get; set; }
    public int CardId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Card Card { get; set; } = null!;
}
