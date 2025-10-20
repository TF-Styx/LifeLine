using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_EducationDocument_TotalHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""EducationDocuments""
                ALTER COLUMN ""TotalHours"" TYPE numeric
                USING (EXTRACT(EPOCH FROM ""TotalHours"") / 3600);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""EducationDocuments""
                ALTER COLUMN ""TotalHours"" TYPE interval
                USING (""TotalHours"" * INTERVAL '1 hour');
            ");
        }
    }
}