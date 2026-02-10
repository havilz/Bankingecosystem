namespace BankingEcosystem.Backend.Models;

public class AtmCashInventory
{
    public int InventoryId { get; set; }
    public int AtmId { get; set; }
    public int Denomination { get; set; }
    public int Quantity { get; set; }

    // Navigation
    public Atm Atm { get; set; } = null!;
}
