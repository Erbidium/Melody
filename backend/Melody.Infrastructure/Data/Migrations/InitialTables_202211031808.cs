using FluentMigrator;

namespace Melody.Infrastructure.Data.Migrations;

[Migration(202211031808)]
public class InitialTables_202211031808 : Migration
{
    public override void Down()
    {
        Delete.Table("PlaylistSongs");
        Delete.Table("UserPlaylists");
        Delete.Table("Playlists");
        Delete.Table("FavouriteSongs");
        Delete.Table("Songs");
        Delete.Table("Genres");
        Delete.Table("UserRoles");
        Delete.Table("UserRefreshTokens");
        Delete.Table("Users");
        Delete.Table("Roles");
    }   
    public override void Up()
    {
        Create.Table("Roles")
           .WithColumn("Id").AsInt64().PrimaryKey().Identity()
           .WithColumn("Name").AsString(50).Nullable()
           .WithColumn("NormalizedName").AsString(50).Nullable();

        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("UserName").AsString(50).Nullable()
            .WithColumn("NormalizedUserName").AsString(50).Nullable()
            .WithColumn("Email").AsString(50).Nullable().Unique()
            .WithColumn("NormalizedEmail").AsString(50).Nullable().Unique()
            .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
            .WithColumn("PasswordHash").AsString().Nullable()
            .WithColumn("PhoneNumber").AsString(50).Nullable().Unique()
            .WithColumn("IsBanned").AsBoolean().NotNullable().WithDefaultValue(0)
            .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(0);

        Create.Table("UserRefreshTokens")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("RefreshToken").AsString().NotNullable().Unique();

        Create.Table("UserRoles")
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("RoleId").AsInt64().NotNullable().ForeignKey("Roles", "Id");

        Create.PrimaryKey().OnTable("UserRoles").Columns("UserId", "RoleId");

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
            .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(0);

        Create.Table("FavouriteSongs")
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("SongId").AsInt64().NotNullable().ForeignKey("Songs", "Id");

        Create.PrimaryKey().OnTable("FavouriteSongs").Columns("UserId", "SongId");

        Create.Table("Playlists")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Link").AsString(50).NotNullable().Unique()
            .WithColumn("AuthorId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(0);

        Create.Table("UserPlaylists")
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("PlaylistId").AsInt64().NotNullable().ForeignKey("Playlists", "Id");

        Create.PrimaryKey().OnTable("UserPlaylists").Columns("UserId", "PlaylistId");

        Create.Table("PlaylistSongs")
            .WithColumn("PlaylistId").AsInt64().NotNullable().ForeignKey("Playlists", "Id")
            .WithColumn("SongId").AsInt64().NotNullable().ForeignKey("Songs", "Id");

        Create.PrimaryKey().OnTable("PlaylistSongs").Columns("PlaylistId", "SongId");
    }
}
