using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialCsvWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CashAndCashEquivalents = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: true),
                    TotalAssets = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: true),
                    TotalLiabilities = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: true),
                    CapitalStock = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: true),
                    CapitalSurplus = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: true),
                    RetainedEarnings = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: true),
                    TotalEquity = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: true),
                    NetWorthPerShare = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: true),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialReports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialReports_CompanyName",
                table: "FinancialReports",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialReports_NetWorthPerShare",
                table: "FinancialReports",
                column: "NetWorthPerShare");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialReports_TotalAssets",
                table: "FinancialReports",
                column: "TotalAssets");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialReports_Year_Quarter_CompanyCode",
                table: "FinancialReports",
                columns: new[] { "Year", "Quarter", "CompanyCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialReports");
        }
    }
}
