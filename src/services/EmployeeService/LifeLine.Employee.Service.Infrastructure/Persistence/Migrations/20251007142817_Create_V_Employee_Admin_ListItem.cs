using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Create_V_Employee_Admin_ListItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var resourceName = "LifeLine.Employee.Service.Infrastructure.Persistence.SQL.Views.V_Employee_Admin_ListItem.sql";
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Не удалось найти встроенный ресурс: {resourceName}");
                }

                using (var reader = new StreamReader(stream))
                {
                    var sqlScript = reader.ReadToEnd();
                    migrationBuilder.Sql(sqlScript);
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Метод Down должен откатывать изменения, т.е. удалять VIEW
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS ""V_Employee_Admin_ListItem"";");
        }
    }
}
