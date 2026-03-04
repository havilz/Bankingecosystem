using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Backend.Models;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Backend.Services;

public class AuthService(BankingDbContext db, IConfiguration config)
{
    public async Task<VerifyCardResponse?> VerifyCardAsync(string cardNumber)
    {
        var card = await db.Cards
            .Include(c => c.Customer)
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

        if (card == null) return null;

        return new VerifyCardResponse(
            card.CardId, card.AccountId, card.Account.AccountNumber,
            card.Customer.FullName, card.IsBlocked);
    }

    public async Task<AuthResponse?> VerifyPinAsync(int cardId, string encryptedPin)
    {
        Console.WriteLine($"[AuthService] VerifyPinAsync received: {encryptedPin}");

        var card = await db.Cards
            .Include(c => c.Customer)
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.CardId == cardId);

        if (card == null || card.IsBlocked) 
        {
            Console.WriteLine($"[AuthService] Card not found or blocked: {cardId}");
            return null;
        }

        string decryptedPin = DecryptPin(encryptedPin);
        Console.WriteLine($"[AuthService] Decrypted PIN: {decryptedPin}");

        if (!BCrypt.Net.BCrypt.Verify(decryptedPin, card.PinHash))
        {
            Console.WriteLine("[AuthService] Hash verification FAILED");
            card.FailedAttempts++;
            if (card.FailedAttempts >= 3) card.IsBlocked = true;
            await db.SaveChangesAsync();
            return null;
        }

        Console.WriteLine("[AuthService] Hash verification SUCCESS");
        card.FailedAttempts = 0;
        await db.SaveChangesAsync();

        var token = GenerateJwt(card.Customer.FullName, card.Account.AccountNumber, "ATMUser");
        return new AuthResponse(token, card.Account.AccountNumber, card.Customer.FullName, card.Account.Balance, card.AccountId);
    }

    public async Task<bool> ChangePinAsync(int cardId, string encryptedOldPin, string encryptedNewPin)
    {
        var card = await db.Cards.FindAsync(cardId);
        if (card == null || card.IsBlocked) return false;

        string decryptedOld = DecryptPin(encryptedOldPin);
        string decryptedNew = DecryptPin(encryptedNewPin);

        if (!BCrypt.Net.BCrypt.Verify(decryptedOld, card.PinHash))
        {
            return false;
        }

        card.PinHash = BCrypt.Net.BCrypt.HashPassword(decryptedNew);
        await db.SaveChangesAsync();
        return true;
    }

    private string DecryptPin(string encryptedPinHex)
    {
        try
        {
            const string Key = "BANK_ECO_SECURE_2026";
            var sb = new StringBuilder();
            
            for (int i = 0; i < encryptedPinHex.Length; i += 2)
            {
                string hexByte = encryptedPinHex.Substring(i, 2);
                byte b = Convert.ToByte(hexByte, 16);
                char decryptedChar = (char)(b ^ Key[(i / 2) % Key.Length]);
                sb.Append(decryptedChar);
            }
            
            return sb.ToString();
        }
        catch
        {
            return string.Empty; // Fail safe
        }
    }

    public async Task<EmployeeLoginResponse?> EmployeeLoginAsync(string employeeCode, string password)
    {
        var emp = await db.Employees
            .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode && e.IsActive);

        if (emp == null || !BCrypt.Net.BCrypt.Verify(password, emp.PasswordHash))
            return null;

        var token = GenerateJwt(emp.FullName, emp.EmployeeCode, emp.Role);
        return new EmployeeLoginResponse(token, emp.FullName, emp.Role);
    }

    private string GenerateJwt(string name, string identifier, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            config["Jwt:Key"] ?? "BankingEcosystemSuperSecretKey2026!@#$%^&*()"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, name),
            new Claim("identifier", identifier),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"] ?? "BankingEcosystem",
            audience: config["Jwt:Audience"] ?? "BankingEcosystemClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(bool Success, string Message, AuthResponse? Data)> RegisterMbankingAsync(
        string cardNumber, string email, DateTime dateOfBirth, string password)
    {
        // 1. Verify card exists
        var card = await db.Cards
            .Include(c => c.Customer)
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

        if (card == null)
            return (false, "Nomor kartu tidak ditemukan.", null);

        if (card.IsBlocked)
            return (false, "Kartu diblokir. Hubungi bank Anda.", null);

        // 2. Verify date of birth matches customer record
        if (card.Customer.DateOfBirth.Date != dateOfBirth.Date)
            return (false, "Tanggal lahir tidak sesuai dengan data kartu.", null);

        // 3. Check email not already registered
        bool emailExists = await db.MbankingAccounts.AnyAsync(m => m.Email == email);
        if (emailExists)
            return (false, "Email sudah terdaftar untuk akun mbanking.", null);

        // 4. Check card not already registered for mbanking
        bool cardRegistered = await db.MbankingAccounts.AnyAsync(m => m.CardId == card.CardId);
        if (cardRegistered)
            return (false, "Kartu ini sudah terdaftar di mbanking.", null);

        // 5. Create mbanking account
        var mbanking = new MbankingAccount
        {
            CardId = card.CardId,
            Email = email.ToLower().Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsActive = true,
            RegisteredAt = DateTime.UtcNow
        };
        db.MbankingAccounts.Add(mbanking);
        await db.SaveChangesAsync();

        var token = GenerateJwt(card.Customer.FullName, card.Account.AccountNumber, "MBankingUser");
        return (true, "Registrasi berhasil.",
            new AuthResponse(token, card.Account.AccountNumber, card.Customer.FullName,
                card.Account.Balance, card.AccountId));
    }

    public async Task<(bool Success, string Message, AuthResponse? Data)> LoginMbankingAsync(
        string email, string password)
    {
        var mbanking = await db.MbankingAccounts
            .Include(m => m.Card)
                .ThenInclude(c => c.Customer)
            .Include(m => m.Card)
                .ThenInclude(c => c.Account)
            .FirstOrDefaultAsync(m => m.Email == email.ToLower().Trim());

        if (mbanking == null)
            return (false, "Email tidak terdaftar.", null);

        if (!mbanking.IsActive)
            return (false, "Akun mbanking tidak aktif.", null);

        if (!BCrypt.Net.BCrypt.Verify(password, mbanking.PasswordHash))
            return (false, "Password salah.", null);

        var card = mbanking.Card;
        if (card.IsBlocked)
            return (false, "Kartu Anda diblokir. Hubungi bank Anda.", null);

        var token = GenerateJwt(card.Customer.FullName, card.Account.AccountNumber, "MBankingUser");
        return (true, "Login berhasil.",
            new AuthResponse(token, card.Account.AccountNumber, card.Customer.FullName,
                card.Account.Balance, card.AccountId));
    }
}
