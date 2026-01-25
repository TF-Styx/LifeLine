using LifeLine.Employee.Service.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_IsActive_For_Employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS \"V_Employee_Hr_Items\";");
            migrationBuilder.Sql("DROP VIEW IF EXISTS \"V_Employee_Full_Details\";");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateView("V_Employee_Hr_Items");
            migrationBuilder.CreateView("V_Employee_Full_Details");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Employees");

            migrationBuilder.DropView("V_Employee_Hr_Items");
            migrationBuilder.DropView("V_Employee_Full_Details");
        }
    }
}
