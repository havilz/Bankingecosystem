using Microsoft.EntityFrameworkCore;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Shared.DTOs;
using BankingEcosystem.Backend.Models;

namespace BankingEcosystem.Backend.Services;

public class AuditLogService(BankingDbContext db)
{
    public async Task LogAsync(int? employeeId, string action, string entityType, int? entityId, string? details = null)
    {
        db.AuditLogs.Add(new AuditLog
        {
            EmployeeId = employeeId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details
        });
        await db.SaveChangesAsync();
    }

    public async Task<List<AuditLogDto>> GetLogsAsync(int page = 1, int pageSize = 50)
    {
        return await db.AuditLogs
            .Include(l => l.Employee)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new AuditLogDto(l.LogId, l.EmployeeId, l.Employee != null ? l.Employee.FullName : null,
                l.Action, l.EntityType, l.EntityId, l.Details, l.CreatedAt))
            .ToListAsync();
    }
}
