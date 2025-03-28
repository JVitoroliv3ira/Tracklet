using FluentMigrator;

namespace Tracklet.Infra.Migrations;

[Migration(2)]
public class V002_CreateDevicesTable : Migration
{
    private const string TableName = "TB_DEVICES";
    
    public override void Up()
    {
        Create.Table(TableName)
            .WithColumn("ID").AsString().PrimaryKey("PK_DEVICES")
            .WithColumn("USER_AGENT").AsString().NotNullable()
            .WithColumn("SCREEN_WIDTH").AsInt32().NotNullable()
            .WithColumn("SCREEN_HEIGHT").AsInt32().NotNullable()
            .WithColumn("OS").AsString().NotNullable()
            .WithColumn("CREATED_AT").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}