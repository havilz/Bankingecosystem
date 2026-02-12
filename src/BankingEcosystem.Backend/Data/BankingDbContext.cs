using Microsoft.EntityFrameworkCore;
using BankingEcosystem.Backend.Models;

namespace BankingEcosystem.Backend.Data;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountType> AccountTypes => Set<AccountType>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionType> TransactionTypes => Set<TransactionType>();
    public DbSet<Atm> Atms => Set<Atm>();
    public DbSet<AtmCashInventory> AtmCashInventories => Set<AtmCashInventory>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ─── Customer ───
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NIK).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(15);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.HasIndex(e => e.NIK).IsUnique();
        });

        // ─── Account ───
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId);
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DailyLimit).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.AccountNumber).IsUnique();

            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Accounts)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AccountType)
                  .WithMany(at => at.Accounts)
                  .HasForeignKey(e => e.AccountTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── AccountType ───
        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.HasKey(e => e.AccountTypeId);
            entity.Property(e => e.TypeName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.MinBalance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DefaultDailyLimit).HasColumnType("decimal(18,2)");

            // Seed data
            entity.HasData(
                new AccountType { AccountTypeId = 1, TypeName = "Savings", MinBalance = 50_000m, DefaultDailyLimit = 10_000_000m },
                new AccountType { AccountTypeId = 2, TypeName = "Checking", MinBalance = 100_000m, DefaultDailyLimit = 25_000_000m },
                new AccountType { AccountTypeId = 3, TypeName = "Business", MinBalance = 500_000m, DefaultDailyLimit = 50_000_000m }
            );
        });

        // ─── Card ───
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.CardId);
            entity.Property(e => e.CardNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PinHash).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.CardNumber).IsUnique();

            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Cards)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Account)
                  .WithMany(a => a.Cards)
                  .HasForeignKey(e => e.AccountId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Transaction ───
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.BalanceBefore).HasColumnType("decimal(18,2)");
            entity.Property(e => e.BalanceAfter).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ReferenceNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TargetAccountNumber).HasMaxLength(20);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.HasIndex(e => e.ReferenceNumber).IsUnique();

            entity.HasOne(e => e.Account)
                  .WithMany(a => a.Transactions)
                  .HasForeignKey(e => e.AccountId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Atm)
                  .WithMany(a => a.Transactions)
                  .HasForeignKey(e => e.AtmId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.TransactionType)
                  .WithMany(tt => tt.Transactions)
                  .HasForeignKey(e => e.TransactionTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── TransactionType ───
        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeId);
            entity.Property(e => e.TypeName).IsRequired().HasMaxLength(50);

            // Seed data
            entity.HasData(
                new TransactionType { TransactionTypeId = 1, TypeName = "Withdrawal" },
                new TransactionType { TransactionTypeId = 2, TypeName = "Deposit" },
                new TransactionType { TransactionTypeId = 3, TypeName = "Transfer" },
                new TransactionType { TransactionTypeId = 4, TypeName = "BalanceInquiry" }
            );
        });

        // ─── ATM ───
        modelBuilder.Entity<Atm>(entity =>
        {
            entity.HasKey(e => e.AtmId);
            entity.Property(e => e.AtmCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TotalCash).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.AtmCode).IsUnique();

            // Seed default ATM
            entity.HasData(new Atm
            {
                AtmId = 1,
                AtmCode = "ATM001",
                Location = "Head Office",
                TotalCash = 100_000_000m,
                IsOnline = true,
                LastRefill = new DateTime(2026, 1, 1)
            });
        });

        // ─── AtmCashInventory ───
        modelBuilder.Entity<AtmCashInventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId);

            entity.HasOne(e => e.Atm)
                  .WithMany(a => a.CashInventory)
                  .HasForeignKey(e => e.AtmId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Employee ───
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.EmployeeCode).IsUnique();

            // Seed: 1 employee (password: admin123)
            entity.HasData(new Employee
            {
                EmployeeId = 1,
                FullName = "Admin Bank",
                EmployeeCode = "ADMIN001",
                PasswordHash = "$2a$11$T.0pM6SsKJuQyMKw1xKv7.rXbiboVxcJfhvw5lSMyIuvhVcabBPga", // admin123
                Role = "Manager",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            });
        });

        // ─── AuditLog ───
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EntityType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Details).HasMaxLength(1000);

            entity.HasOne(e => e.Employee)
                  .WithMany(e => e.AuditLogs)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
