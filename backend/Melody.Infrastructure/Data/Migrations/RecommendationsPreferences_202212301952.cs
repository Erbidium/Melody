using FluentMigrator;

namespace Melody.Infrastructure.Data.Migrations;

[Migration(202212301952)]
public class RecommendationsPreferences_202212301952 : Migration
{
    public override void Down()
    {
        Delete.Table("RecommendationsPreferences");
    }

    public override void Up()
    {
        Create.Table("RecommendationsPreferences")
            .WithColumn("UserId").AsInt64().NotNullable().Unique().ForeignKey("Users", "Id")
            .WithColumn("AuthorName").AsString(50).Nullable()
            .WithColumn("StartYear").AsInt32().Nullable()
            .WithColumn("EndYear").AsInt32().Nullable()
            .WithColumn("GenreId").AsInt64().NotNullable().ForeignKey("Genres", "Id")
            .WithColumn("AverageDurationInMinutes").AsInt32().Nullable();
    }
}