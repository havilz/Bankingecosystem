namespace BankingEcosystem.Backend.Models;

public class AccountType
{
    public int AccountTypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public decimal MinBalance { get; set; }
    public decimal DefaultDailyLimit { get; set; }

    // Navigation
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
