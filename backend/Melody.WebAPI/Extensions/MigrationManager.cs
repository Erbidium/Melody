using FluentMigrator.Runner;
using Melody.Infrastructure.Data.Migrations;

namespace Melody.WebAPI.Extensions;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApplicationBuilder)
    {
        using (var scope = webApplicationBuilder.Services.CreateScope())
        {
            var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
            var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            databaseService.CreateDatabase("DapperMigrationExample");

            migrationService.ListMigrations();
            migrationService.MigrateUp();
        }
        return webApplicationBuilder;
    }
}
