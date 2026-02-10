using System.Runtime.InteropServices;

namespace BankingEcosystem.Interop;

/// <summary>
/// P/Invoke wrapper for BankingEcosystem.Hardware.dll (C++ ATM Hardware Simulation)
/// </summary>
public static class HardwareInterop
{
    private const string HardwareDll = "BankingEcosystem.Hardware.dll";

    // Card Reader
    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int CardReader_Insert([MarshalAs(UnmanagedType.LPStr)] string cardNumber);

    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int CardReader_Eject();

    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int CardReader_GetCardNumber([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder buffer, int maxLen);

    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int CardReader_IsCardInserted();

    // Cash Dispenser
    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int CashDispenser_Dispense(int amount);

    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int CashDispenser_GetRemainingCash();

    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int CashDispenser_Refill(int amount);

    // Receipt Printer
    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int ReceiptPrinter_Print([MarshalAs(UnmanagedType.LPStr)] string receiptData);

    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int ReceiptPrinter_IsReady();

    // Keypad
    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Keypad_ReadPin([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder buffer, int maxLen);

    [DllImport(HardwareDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Keypad_IsActive();
}

/// <summary>
/// P/Invoke wrapper for BankingEcosystem.NativeLogic.dll (C++ ATM Logic)
/// </summary>
public static class NativeLogicInterop
{
    private const string LogicDll = "BankingEcosystem.NativeLogic.dll";

    // ATM State Machine
    [DllImport(LogicDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int AtmFsm_GetState();

    [DllImport(LogicDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int AtmFsm_TransitionTo(int newState);

    [DllImport(LogicDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int AtmFsm_Reset();

    [DllImport(LogicDll, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.LPStr)]
    public static extern string AtmFsm_GetStateName(int state);

    // Transaction Validator
    [DllImport(LogicDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Validator_CheckWithdrawalLimit(double amount, double dailyLimit, double alreadyWithdrawn);

    [DllImport(LogicDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Validator_CheckSufficientBalance(double balance, double amount, double minBalance);

    [DllImport(LogicDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Validator_CheckTransferAmount(double amount);

    // Encryption
    [DllImport(LogicDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Encryption_HashPin(
        [MarshalAs(UnmanagedType.LPStr)] string pin,
        [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder hashOutput,
        int maxLen);

    [DllImport(LogicDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Encryption_VerifyPin(
        [MarshalAs(UnmanagedType.LPStr)] string pin,
        [MarshalAs(UnmanagedType.LPStr)] string hash);
}
