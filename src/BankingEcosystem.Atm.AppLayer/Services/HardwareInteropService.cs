using System.Runtime.InteropServices;
using System.Text;
using BankingEcosystem.Interop;

namespace BankingEcosystem.Atm.AppLayer.Services;

public interface IHardwareInteropService
{
    string? ReadCard();
    bool EjectCard();
    bool IsCardInserted();
    bool DispenseCash(int amount);
    int GetRemainingCash();
    bool PrintReceipt(string receiptData);
}

public class HardwareInteropService : IHardwareInteropService
{
    public string? ReadCard()
    {
        try
        {
            var sb = new StringBuilder(20);
            int result = HardwareInterop.CardReader_GetCardNumber(sb, sb.Capacity);
            return result == 0 ? sb.ToString() : null; // Assuming 0 is success
        }
        catch (DllNotFoundException)
        {
            // Simulation fallback or log error
            return null;
        }
    }

    public bool EjectCard()
    {
        try
        {
            return HardwareInterop.CardReader_Eject() == 0;
        }
        catch
        {
            return false;
        }
    }

    public bool IsCardInserted()
    {
        try
        {
            return HardwareInterop.CardReader_IsCardInserted() == 1; // Assuming 1 is true
        }
        catch
        {
            return false;
        }
    }

    public bool DispenseCash(int amount)
    {
        try
        {
            return HardwareInterop.CashDispenser_Dispense(amount) == 0;
        }
        catch
        {
            return false;
        }
    }

    public int GetRemainingCash()
    {
        try
        {
            return HardwareInterop.CashDispenser_GetRemainingCash();
        }
        catch
        {
            return -1;
        }
    }

    public bool PrintReceipt(string receiptData)
    {
        try
        {
            return HardwareInterop.ReceiptPrinter_Print(receiptData) == 0;
        }
        catch
        {
            return false;
        }
    }
}
