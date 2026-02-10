using Microsoft.EntityFrameworkCore;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Backend.Models;

namespace BankingEcosystem.Backend.Services;

public class AdminService(BankingDbContext db)
{
    // ─── Customer Management ───
    public async Task<List<CustomerDto>> GetAllCustomersAsync()
    {
        return await db.Customers
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CustomerDto(c.CustomerId, c.FullName, c.NIK, c.Phone, c.Email, c.Address, c.DateOfBirth, c.CreatedAt))
            .ToListAsync();
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var c = await db.Customers.FindAsync(id);
        return c == null ? null : new CustomerDto(c.CustomerId, c.FullName, c.NIK, c.Phone, c.Email, c.Address, c.DateOfBirth, c.CreatedAt);
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            FullName = request.FullName,
            NIK = request.NIK,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            DateOfBirth = request.DateOfBirth
        };

        db.Customers.Add(customer);
        await db.SaveChangesAsync();

        return new CustomerDto(customer.CustomerId, customer.FullName, customer.NIK,
            customer.Phone, customer.Email, customer.Address, customer.DateOfBirth, customer.CreatedAt);
    }

    public async Task<bool> UpdateCustomerAsync(int id, UpdateCustomerRequest request)
    {
        var customer = await db.Customers.FindAsync(id);
        if (customer == null) return false;

        customer.FullName = request.FullName;
        customer.Phone = request.Phone;
        customer.Email = request.Email;
        customer.Address = request.Address;
        await db.SaveChangesAsync();
        return true;
    }

    // ─── ATM Management ───
    public async Task<List<AtmDto>> GetAllAtmsAsync()
    {
        return await db.Atms
            .Select(a => new AtmDto(a.AtmId, a.AtmCode, a.Location, a.IsOnline, a.TotalCash, a.LastRefill))
            .ToListAsync();
    }

    public async Task<AtmDto> CreateAtmAsync(CreateAtmRequest request)
    {
        var atm = new Atm
        {
            AtmCode = request.AtmCode,
            Location = request.Location,
            TotalCash = request.InitialCash,
            IsOnline = true,
            LastRefill = DateTime.UtcNow
        };

        db.Atms.Add(atm);
        await db.SaveChangesAsync();

        return new AtmDto(atm.AtmId, atm.AtmCode, atm.Location, atm.IsOnline, atm.TotalCash, atm.LastRefill);
    }

    public async Task<bool> RefillAtmAsync(RefillAtmRequest request)
    {
        var atm = await db.Atms.FindAsync(request.AtmId);
        if (atm == null) return false;

        var inventory = await db.AtmCashInventories
            .FirstOrDefaultAsync(i => i.AtmId == request.AtmId && i.Denomination == request.Denomination);

        if (inventory != null)
        {
            inventory.Quantity += request.Quantity;
        }
        else
        {
            db.AtmCashInventories.Add(new AtmCashInventory
            {
                AtmId = request.AtmId,
                Denomination = request.Denomination,
                Quantity = request.Quantity
            });
        }

        atm.TotalCash += request.Denomination * request.Quantity;
        atm.LastRefill = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleAtmStatusAsync(int atmId)
    {
        var atm = await db.Atms.FindAsync(atmId);
        if (atm == null) return false;
        atm.IsOnline = !atm.IsOnline;
        await db.SaveChangesAsync();
        return true;
    }

    // ─── Employee Management ───
    public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
    {
        return await db.Employees
            .Select(e => new EmployeeDto(e.EmployeeId, e.FullName, e.EmployeeCode, e.Role, e.IsActive, e.CreatedAt))
            .ToListAsync();
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeRequest request)
    {
        var emp = new Employee
        {
            FullName = request.FullName,
            EmployeeCode = request.EmployeeCode,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role
        };

        db.Employees.Add(emp);
        await db.SaveChangesAsync();

        return new EmployeeDto(emp.EmployeeId, emp.FullName, emp.EmployeeCode, emp.Role, emp.IsActive, emp.CreatedAt);
    }
}
