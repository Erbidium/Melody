using FluentMigrator;
using FluentMigrator.SqlServer;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.DbEntites;

namespace Melody.Infrastructure.Data.Migrations;

[Migration(202212121507)]
public class AddMusicGenres_202212121507 : Migration
{
    public override void Down()
    {
        Delete.FromTable("Genres")
            .Row(new GenreDb(13, "Hip hop"))
            .Row(new GenreDb(14, "Soul"))
            .Row(new GenreDb(15, "Funk"))
            .Row(new GenreDb(16, "Folk"))
            .Row(new GenreDb(17, "Disco"))
            .Row(new GenreDb(18, "Classical"))
            .Row(new GenreDb(19, "Metal"))
            .Row(new GenreDb(20, "Punk"));
    }

    public override void Up()
    {
        Insert.IntoTable("Genres")
            .WithIdentityInsert()
            .Row(new GenreDb(13, "Hip hop"))
            .Row(new GenreDb(14, "Soul"))
            .Row(new GenreDb(15, "Funk"))
            .Row(new GenreDb(16, "Folk"))
            .Row(new GenreDb(17, "Disco"))
            .Row(new GenreDb(18, "Classical"))
            .Row(new GenreDb(19, "Metal"))
            .Row(new GenreDb(20, "Punk"));
    }
}