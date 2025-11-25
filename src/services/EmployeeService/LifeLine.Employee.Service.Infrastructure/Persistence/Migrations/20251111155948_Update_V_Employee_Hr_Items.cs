using LifeLine.Employee.Service.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_V_Employee_Hr_Items : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateView("V_Employee_Hr_Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropView("V_Employee_Hr_Items");
        }
    }
}
