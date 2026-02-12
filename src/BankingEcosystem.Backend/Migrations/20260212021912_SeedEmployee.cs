using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingEcosystem.Backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "CreatedAt", "EmployeeCode", "FullName", "IsActive", "PasswordHash", "Role" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN001", "Admin Bank", true, "$2a$11$T.0pM6SsKJuQyMKw1xKv7.rXbiboVxcJfhvw5lSMyIuvhVcabBPga", "Manager" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1);
        }
    }
}
