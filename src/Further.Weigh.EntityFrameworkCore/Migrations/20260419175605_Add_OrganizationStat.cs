using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Further.Weigh.Migrations
{
    /// <inheritdoc />
    public partial class Add_OrganizationStat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupCount = table.Column<int>(type: "int", nullable: false),
                    TotalMembers = table.Column<int>(type: "int", nullable: false),
                    StaffTotal = table.Column<int>(type: "int", nullable: false),
                    VolunteerTotal = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationStats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationStats");
        }
    }
}
