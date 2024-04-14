using FluentMigrator;

namespace CrudEmployee.Infrastructure.Database.Migrations;

[Migration(20180430121800)]
public class AddEmployeeTables : Migration
{
    public override void Up()
    {
        Create.Table("Employee")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Age").AsInt16().NotNullable()
            .WithColumn("Salary").AsDecimal(12,2).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Employee");
    }
}
