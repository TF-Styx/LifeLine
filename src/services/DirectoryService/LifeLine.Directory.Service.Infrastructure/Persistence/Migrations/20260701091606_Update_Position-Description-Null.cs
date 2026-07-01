using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_PositionDescriptionNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Positions",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "character varying(5000)",
                oldMaxLength: 5000,
                oldCollation: "case_insensitive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Positions",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "",
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "character varying(5000)",
                oldMaxLength: 5000,
                oldNullable: true,
                oldCollation: "case_insensitive");
        }
    }
}
