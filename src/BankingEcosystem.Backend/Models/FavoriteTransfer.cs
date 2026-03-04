namespace BankingEcosystem.Backend.Models;

public class FavoriteTransfer
{
    public int FavoriteId { get; set; }
    public int AccountId { get; set; }
    public string FavoriteAccountNumber { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Account Account { get; set; } = null!;
}
