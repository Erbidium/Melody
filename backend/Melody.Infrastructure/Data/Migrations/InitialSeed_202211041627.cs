using FluentMigrator;
using FluentMigrator.SqlServer;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.DbEntites;

namespace Melody.Infrastructure.Data.Migrations;

[Migration(202211041627)]
public class InitialSeed_202211041627 : Migration
{
    public override void Down()
    {
        Delete.FromTable("Roles")
            .Row(new RoleIdentity { Id = 0, Name = "User", NormalizedName = "user" })
            .Row(new RoleIdentity { Id = 1, Name = "Admin", NormalizedName = "admin" });

        Delete.FromTable("Genres")
            .Row(new GenreDb(0, "Rock"))
            .Row(new GenreDb(1, "Jazz"))
            .Row(new GenreDb(2, "Rap"))
            .Row(new GenreDb(3, "Country"))
            .Row(new GenreDb(4, "Rock and Roll"))
            .Row(new GenreDb(5, "Pop"))
            .Row(new GenreDb(6, "Electronic"))
            .Row(new GenreDb(7, "Techno"))
            .Row(new GenreDb(8, "Dubstep"))
            .Row(new GenreDb(9, "Rhythm and Blues"))
            .Row(new GenreDb(10, "Indie"))
            .Row(new GenreDb(11, "Reggae"))
            .Row(new GenreDb(12, "Other"));
    }

    public override void Up()
    {
        Insert.IntoTable("Roles")
            .WithIdentityInsert()
            .Row(new RoleIdentity { Id = 0, Name = "User", NormalizedName = "user" })
            .Row(new RoleIdentity { Id = 1, Name = "Admin", NormalizedName = "admin" });

        Insert.IntoTable("Genres")
            .WithIdentityInsert()
            .Row(new GenreDb(0, "Rock"))
            .Row(new GenreDb(1, "Jazz"))
            .Row(new GenreDb(2, "Rap"))
            .Row(new GenreDb(3, "Country"))
            .Row(new GenreDb(4, "Rock and Roll"))
            .Row(new GenreDb(5, "Pop"))
            .Row(new GenreDb(6, "Electronic"))
            .Row(new GenreDb(7, "Techno"))
            .Row(new GenreDb(8, "Dubstep"))
            .Row(new GenreDb(9, "Rhythm and Blues"))
            .Row(new GenreDb(10, "Indie"))
            .Row(new GenreDb(11, "Reggae"))
            .Row(new GenreDb(12, "Other"));
    }
}
