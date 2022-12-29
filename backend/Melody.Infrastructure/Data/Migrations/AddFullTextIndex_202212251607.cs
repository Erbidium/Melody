using FluentMigrator;

namespace Melody.Infrastructure.Data.Migrations;

[Migration(202212251607, TransactionBehavior.None)]
public class AddFullTextIndex_202212251607 : Migration
{
    public override void Down()
    {
        Execute.Sql(@"
            DROP FULLTEXT INDEX ON Songs;
            DROP FULLTEXT CATALOG ft;    
        ");
    }

    public override void Up()
    {
        Execute.Sql(@"
            CREATE FULLTEXT CATALOG ft AS DEFAULT;
            CREATE FULLTEXT INDEX ON Songs(Name)
                KEY INDEX PK_Songs;
        ");
    }
}
