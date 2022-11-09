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
            .Row(new RoleRecord(0, "User", "user"))
            .Row(new RoleRecord(1, "Admin", "admin"));

        Delete.FromTable("Roles")
            .Row(new GenreRecord(0, "Rock"))
            .Row(new GenreRecord(1, "Jazz"))
            .Row(new GenreRecord(2, "Rap"))
            .Row(new GenreRecord(3, "Country"))
            .Row(new GenreRecord(4, "Rock and Roll"))
            .Row(new GenreRecord(5, "Pop"))
            .Row(new GenreRecord(6, "Electronic"))
            .Row(new GenreRecord(7, "Techno"))
            .Row(new GenreRecord(8, "Dubstep"))
            .Row(new GenreRecord(9, "Rhythm and Blues"))
            .Row(new GenreRecord(10, "Indie"))
            .Row(new GenreRecord(11, "Reggae"))
            .Row(new GenreRecord(12, "Other"));
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
            .Row(new GenreRecord(1, "Jazz"))
            .Row(new GenreRecord(2, "Rap"))
            .Row(new GenreRecord(3, "Country"))
            .Row(new GenreRecord(4, "Rock and Roll"))
            .Row(new GenreRecord(5, "Pop"))
            .Row(new GenreRecord(6, "Electronic"))
            .Row(new GenreRecord(7, "Techno"))
            .Row(new GenreRecord(8, "Dubstep"))
            .Row(new GenreRecord(9, "Rhythm and Blues"))
            .Row(new GenreRecord(10, "Indie"))
            .Row(new GenreRecord(11, "Reggae"))
            .Row(new GenreRecord(12, "Other"));
    }
}
