using FluentMigrator;

namespace CC.TheBench.Frontend.Migrations
{
    [Migration(201401082219)]
    public class CreateUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users");
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}