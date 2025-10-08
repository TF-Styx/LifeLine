using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateContactInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonalPhone = table.Column<string>(type: "text", nullable: false),
                    CorporatePhone = table.Column<string>(type: "text", nullable: true),
                    PersonalEmail = table.Column<string>(type: "text", nullable: false),
                    CorporateEmail = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Building = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Apartment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInformations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformations_EmployeeId",
                table: "ContactInformations",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInformations");
        }
    }
}
