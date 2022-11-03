using FluentMigrator;
using FluentMigrator.SqlServer;

namespace Melody.Infrastructure.Data.Migrations;

public class InitialTables_202211031808 : Migration
{
    public override void Down()
    {
        Delete.Table("FavouriteSongs");
        Delete.Table("UserPlaylists");
        Delete.Table("Playlists");
        Delete.Table("PlaylistSongs");
    }   
    public override void Up()
    {
        Create.Table("FavouriteSongs")
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("SongId").AsInt64().NotNullable().ForeignKey("Songs", "Id");

        Create.PrimaryKey().OnTable("FavouriteSongs").Columns("UserId", "SongId");

        Create.Table("UserPlaylists")
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("PlaylistId").AsInt64().NotNullable().ForeignKey("Playlists", "Id");

        Create.PrimaryKey().OnTable("UserPlaylists").Columns("UserId", "PlaylistId");

        Create.Table("Playlists")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Link").AsString(50).NotNullable()
            .WithColumn("AuthorId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("IsDeleted").AsBoolean().NotNullable();

        Create.Table("PlaylistSongs")
            .WithColumn("PlaylistId").AsInt64().NotNullable().ForeignKey("Playlists", "Id")
            .WithColumn("SongId").AsInt64().NotNullable().ForeignKey("Songs", "Id");

        Create.PrimaryKey().OnTable("PlaylistSongs").Columns("PlaylistId", "SongId");

        Create.Table("Genres")
           .WithColumn("Id").AsInt64().PrimaryKey().Identity()
           .WithColumn("Name").AsString(50).NotNullable();

        Create.Table("Songs")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Path").AsString(50).NotNullable()
            .WithColumn("AuthorName").AsString(50).NotNullable()
            .WithColumn("Year").AsInt32().NotNullable()
            .WithColumn("GenreId").AsInt64().NotNullable().ForeignKey("Genres", "Id")
            .WithColumn("IsDeleted").AsBoolean().NotNullable();

        Create.Table("Roles")
           .WithColumn("Id").AsInt64().PrimaryKey().Identity()
           .WithColumn("Name").AsString(50).NotNullable();

        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Email").AsString(50).NotNullable()
            .WithColumn("PhoneNumber").AsString(50).NotNullable()
            .WithColumn("RoleId").AsInt64().NotNullable().ForeignKey("Roles", "Id")
            .WithColumn("IsBanned").AsBoolean().NotNullable()
            .WithColumn("IsDeleted").AsBoolean().NotNullable();
    }
}
