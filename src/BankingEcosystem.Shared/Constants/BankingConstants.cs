namespace BankingEcosystem.Shared.Constants;

public static class BankingConstants
{
    // PIN
    public const int PinLength = 6;
    public const int MaxPinAttempts = 3;

    // Transaction Limits (in Rupiah)
    public const decimal DefaultDailyWithdrawalLimit = 10_000_000m;
    public const decimal MaxTransferAmount = 100_000_000m;
    public const decimal MinWithdrawalAmount = 50_000m;

    // Denominations
    public static readonly int[] AvailableDenominations = { 100_000, 50_000 };

    // Account
    public const decimal DefaultMinBalance = 50_000m;

    // ATM
    public const decimal DefaultAtmCash = 50_000_000m;
}
