using FluentMigrator;

namespace Tracklet.Infra.Migrations;

[Migration(1)]
public class V001_CreateVisitorsTable : Migration{
    private const string TableName = "TB_VISITORS";
    
    public override void Up()
    {
        Create.Table(TableName)
            .WithColumn("ID").AsString().PrimaryKey("PK_VISITORS")
            .WithColumn("FIRST_SEEN").AsDateTime().NotNullable()
            .WithColumn("LAST_SEEN").AsDateTime().NotNullable()
            .WithColumn("CREATED_AT").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}