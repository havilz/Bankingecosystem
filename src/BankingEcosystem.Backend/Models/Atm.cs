namespace BankingEcosystem.Backend.Models;

public class Atm
{
    public int AtmId { get; set; }
    public string AtmCode { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsOnline { get; set; } = true;
    public decimal TotalCash { get; set; }
    public DateTime? LastRefill { get; set; }

    // Navigation
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<AtmCashInventory> CashInventory { get; set; } = new List<AtmCashInventory>();
}
