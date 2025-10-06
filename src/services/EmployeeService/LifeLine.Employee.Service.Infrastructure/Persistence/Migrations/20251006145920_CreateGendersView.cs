using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateGendersView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Полное имя ресурса формируется как: {Пространствоименпоумолчанию}.{Путькфайлучерез_точки}
            // Например: LifeLine.Employee.Service.Infrastructure.Persistence.SQL.Views.V_Genders.sql
            var resourceName = "LifeLine.Employee.Service.Infrastructure.Persistence.SQL.Views.V_Genders.sql";
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
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS ""V_Genders"";");
        }
    }
}
