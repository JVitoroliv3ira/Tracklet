using FluentMigrator;

namespace Tracklet.Infra.Migrations;

[Migration(3)]
public class V003_CreateSessionsTable : Migration
{
    private const string TableName = "TB_SESSIONS";
    
    public override void Up()
    {
        Create.Table(TableName)
            .WithColumn("ID").AsString().PrimaryKey("PK_SESSIONS")
            .WithColumn("VISITOR_ID").AsString().NotNullable().ForeignKey("FK_SESSIONS_VISITORS", "TB_VISITORS", "ID")
            .WithColumn("DEVICE_ID").AsString().NotNullable().ForeignKey("FK_SESSIONS_DEVICES", "TB_DEVICES", "ID")
            .WithColumn("START_TIME").AsDateTime().NotNullable()
            .WithColumn("END_TIME").AsDateTime().NotNullable()
            .WithColumn("DURATION").AsInt64().NotNullable()
            .WithColumn("CREATED_AT").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}