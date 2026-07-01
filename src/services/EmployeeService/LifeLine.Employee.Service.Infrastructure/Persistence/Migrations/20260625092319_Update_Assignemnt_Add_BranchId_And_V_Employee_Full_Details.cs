using LifeLine.Employee.Service.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Assignemnt_Add_BranchId_And_V_Employee_Full_Details : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "Assignments",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);
            migrationBuilder.CreateView("V_Employee_Full_Details");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Assignments");
            migrationBuilder.DropView("V_Employee_Full_Details");
        }
    }
}
