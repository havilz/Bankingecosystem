namespace BankingEcosystem.Backend.Models;

public class TransactionType
{
    public int TransactionTypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;

    // Navigation
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
