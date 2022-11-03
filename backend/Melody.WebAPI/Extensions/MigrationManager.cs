using Melody.Infrastructure.Data.Migrations;

namespace Melody.WebAPI.Extensions;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApplicationBuilder)
    {
        using (var scope = webApplicationBuilder.Services.CreateScope())
        {
            var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
            databaseService.CreateDatabase("DapperMigrationExample");
        }
        return webApplicationBuilder;
    }
}
