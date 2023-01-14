using FluentMigrator;

namespace Melody.Infrastructure.Data.Migrations;

[Migration(202301122316)]
public class AddPrimaryKeyToRecommendationsPreferences_202301122316 : Migration
{
    public override void Down()
    {
        Delete
            .PrimaryKey("PK_RecommendationsPreferences")
            .FromTable("RecommendationsPreferences");
    }

    public override void Up()
    {
        Create
            .PrimaryKey("PK_RecommendationsPreferences")
            .OnTable("RecommendationsPreferences")
            .Column("UserId");
    }
}
