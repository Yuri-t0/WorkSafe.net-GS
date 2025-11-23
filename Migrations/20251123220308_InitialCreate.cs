using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkSafe.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workstations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MonitorDistanceCm = table.Column<int>(type: "int", nullable: false),
                    HasAdjustableChair = table.Column<bool>(type: "bit", nullable: false),
                    HasFootrest = table.Column<bool>(type: "bit", nullable: false),
                    ErgonomicRiskLevel = table.Column<int>(type: "int", nullable: false),
                    LastEvaluationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workstations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workstations");
        }
    }
}
