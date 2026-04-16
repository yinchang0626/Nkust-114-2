using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Further.Weigh.Migrations
{
    /// <inheritdoc />
    public partial class _0011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTransportContracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ContractType = table.Column<int>(type: "int", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FormDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTransportContracts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppTransportContracts_TenantId_Code",
                table: "AppTransportContracts",
                columns: new[] { "TenantId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_AppTransportContracts_TenantId_Status",
                table: "AppTransportContracts",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppTransportContracts_TenantId_ValidFrom_ValidTo",
                table: "AppTransportContracts",
                columns: new[] { "TenantId", "ValidFrom", "ValidTo" });

            migrationBuilder.CreateIndex(
                name: "IX_AppTransportContracts_TenantId_VendorId",
                table: "AppTransportContracts",
                columns: new[] { "TenantId", "VendorId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTransportContracts");
        }
    }
}
