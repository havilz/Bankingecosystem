using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingEcosystem.Backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedAtm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Atms",
                columns: new[] { "AtmId", "AtmCode", "IsOnline", "LastRefill", "Location", "TotalCash" },
                values: new object[] { 1, "ATM001", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Head Office", 100000000m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Atms",
                keyColumn: "AtmId",
                keyValue: 1);
        }
    }
}
