using FluentMigrator;

namespace CC.TheBench.Frontend.Migrations
{
    [Migration(201401092104)]
    public class CreateUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("UserId").AsGuid().NotNullable().Indexed("UserId")
                .WithColumn("Email").AsString(255).NotNullable().PrimaryKey("Email")
                .WithColumn("Hash").AsFixedLengthString(64).NotNullable()
                .WithColumn("Salt").AsFixedLengthString(8).NotNullable()
                .WithColumn("DisplayName").AsString(100).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}
