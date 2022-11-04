using FluentMigrator;
using FluentMigrator.SqlServer;
using Melody.Infrastructure.Data.Records;

namespace Melody.Infrastructure.Data.Migrations;

[Migration(202211041627)]
public class InitialSeed_202211041627 : Migration
{
    public override void Down()
    {
        Delete.FromTable("Roles")
            .Row(new RoleRecord(0, "User"))
            .Row(new RoleRecord(1, "Admin"));

        Delete.FromTable("Roles")
            .Row(new GenreRecord(0, "Rock"))
            .Row(new GenreRecord(1, "Indie"))
            .Row(new GenreRecord(2, "Rap"));
    }

    public override void Up()
    {
        Insert.IntoTable("Roles")
            .WithIdentityInsert()
            .Row(new RoleRecord(0, "User"))
            .Row(new RoleRecord(1, "Admin"));

        Insert.IntoTable("Genres")
            .WithIdentityInsert()
            .Row(new GenreRecord(0, "Rock"))
            .Row(new GenreRecord(1, "Indie"))
            .Row(new GenreRecord(2, "Rap"));
    }
}
