using FluentMigrator;

namespace Melody.Infrastructure.Data.Migrations;

[Migration(202212291503, TransactionBehavior.None)]
public class AddFullTextIndexUsers_202212291503 : Migration
{
    public override void Down()
    {
        Execute.Sql(@"
            DROP FULLTEXT INDEX ON Users; 
        ");
    }

    public override void Up()
    {
        Execute.Sql(@"
            CREATE FULLTEXT INDEX ON Users(UserName)
                KEY INDEX PK_Users;
        ");
    }
}
