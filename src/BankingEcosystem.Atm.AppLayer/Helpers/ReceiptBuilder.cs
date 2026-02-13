using System.Text;

namespace BankingEcosystem.Atm.AppLayer.Helpers;

public class ReceiptBuilder
{
    private readonly StringBuilder _sb = new();
    private const int Width = 40;

    public ReceiptBuilder AddHeader(string bankName, string atmId)
    {
        _sb.AppendLine(CenterText(bankName));
        _sb.AppendLine(CenterText("ATM RECEIPT"));
        _sb.AppendLine(new string('-', Width));
        _sb.AppendLine($"Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        _sb.AppendLine($"ATM ID: {atmId}");
        _sb.AppendLine(new string('-', Width));
        return this;
    }

    public ReceiptBuilder AddBody(string label, string value)
    {
        // Simple key-value alignment
        // Label............Value
        if (label.Length + value.Length + 2 > Width)
        {
            _sb.AppendLine(label);
            _sb.AppendLine(value.PadLeft(Width));
        }
        else
        {
            int dots = Width - label.Length - value.Length;
            _sb.AppendLine($"{label}{new string('.', dots)}{value}");
        }
        return this;
    }

    public ReceiptBuilder AddSeparator()
    {
        _sb.AppendLine(new string('-', Width));
        return this;
    }

    public ReceiptBuilder AddFooter(string message)
    {
        _sb.AppendLine(new string('=', Width));
        _sb.AppendLine(CenterText(message));
        _sb.AppendLine(CenterText("Thank you for banking with us."));
        return this;
    }

    public override string ToString()
    {
        return _sb.ToString();
    }

    private string CenterText(string text)
    {
        if (text.Length >= Width) return text;
        int padding = (Width - text.Length) / 2;
        return text.PadLeft(text.Length + padding).PadRight(Width);
    }
}
