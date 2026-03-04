using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingEcosystem.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMbankingFavoritesBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteTransfers",
                columns: table => new
                {
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FavoriteAccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteTransfers", x => x.FavoriteId);
                    table.ForeignKey(
                        name: "FK_FavoriteTransfers_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MbankingAccounts",
                columns: table => new
                {
                    MbankingAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbankingAccounts", x => x.MbankingAccountId);
                    table.ForeignKey(
                        name: "FK_MbankingAccounts_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "BankId", "BankCode", "BankName" },
                values: new object[,]
                {
                    { 1, "BCA", "Bank Central Asia (BCA)" },
                    { 2, "BRI", "Bank Rakyat Indonesia (BRI)" },
                    { 3, "BNI", "Bank Negara Indonesia (BNI)" },
                    { 4, "MANDIRI", "Bank Mandiri" },
                    { 5, "BSI", "Bank Syariah Indonesia (BSI)" },
                    { 6, "CIMB", "CIMB Niaga" },
                    { 7, "DANAMON", "Bank Danamon" },
                    { 8, "PERMATA", "Bank Permata" },
                    { 9, "BTN", "Bank Tabungan Negara (BTN)" },
                    { 10, "MEGA", "Bank Mega" },
                    { 11, "OCBC", "OCBC Indonesia" },
                    { 12, "PANIN", "Bank Panin" },
                    { 13, "MAYBANK", "Maybank Indonesia" },
                    { 14, "JAGO", "Bank Jago" },
                    { 15, "ECOSYS", "Banking Ecosystem (Internal)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteTransfers_AccountId",
                table: "FavoriteTransfers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MbankingAccounts_CardId",
                table: "MbankingAccounts",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_MbankingAccounts_Email",
                table: "MbankingAccounts",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "FavoriteTransfers");

            migrationBuilder.DropTable(
                name: "MbankingAccounts");
        }
    }
}
