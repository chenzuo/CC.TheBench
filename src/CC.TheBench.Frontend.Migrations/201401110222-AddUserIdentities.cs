using FluentMigrator;

namespace CC.TheBench.Frontend.Migrations
{
    using Web.Data.ReadModel;

    [Migration(201401110222)]
    public class AddUserIdentities : Migration
    {
        public override void Up()
        {
            Create.Table("UserIdentities")
                .WithColumn("UserId").AsGuid().NotNullable().Indexed("UserId")
                .WithColumn("Provider").AsString(10).NotNullable()
                .WithColumn("Id").AsString(20).NotNullable()
                .WithColumn("Email").AsString(255).Nullable()
                .WithColumn("Locale").AsString(10).Nullable()
                .WithColumn("Name").AsString(100).Nullable()
                .WithColumn("Picture").AsString(200).Nullable()
                .WithColumn("Username").AsString(50).Nullable();

            Create.Index("Provider-Id")
                .OnTable("UserIdentities")
                .OnColumn("Provider").Ascending()
                .OnColumn("Id").Ascending();

            Insert.IntoTable("UserIdentities").Row(new UserIdentity
            {
                UserId = Constants.DummyUserId.ToString(),
                Provider = "facebook",
                Id = "727564408",
                Email = "david@cumps.be",
                Locale = "en_US",
                Name = "David Cumps",
                Picture = "https://graph.facebook.com/727564408/picture",
                Username = "cumpsd"
            });
        }

        public override void Down()
        {
            Delete.Table("UserIdentities");
        }
    }
}