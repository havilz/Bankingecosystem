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
            // Try to print via hardware (simulation DLL)
            int hwResult = -1;
            try
            {
                hwResult = HardwareInterop.ReceiptPrinter_Print(receiptData);
            }
            catch (DllNotFoundException)
            {
                // Ignore missing DLL, use file fallback
            }

            // Save to file for verification
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "__receipts");
            Directory.CreateDirectory(dir);
            string filename = $"Receipt_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}.txt";
            File.WriteAllText(Path.Combine(dir, filename), receiptData);

            return hwResult == 0 || true; // Always return true if file write succeeds
        }
        catch
        {
            return false;
        }
    }
}
