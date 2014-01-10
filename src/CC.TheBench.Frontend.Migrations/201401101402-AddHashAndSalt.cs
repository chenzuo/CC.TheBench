using FluentMigrator;

namespace CC.TheBench.Frontend.Migrations
{
    using System;
    using Web.Data.ReadModel;
    using Web.Security;

    [Migration(201401101402)]
    public class AddHashAndSalt : Migration
    {
        public override void Up()
        {
            Delete.Column("Hash").FromTable("Users");
            Delete.Column("Salt").FromTable("Users");

            Alter.Table("Users")
                .AddColumn("HashAndSalt").AsFixedLengthString(192).NotNullable();

            Delete.FromTable("Users").Row(new { Email = "david@cumps.be" });

            ISaltedHash saltedHash = new SaltedHash();
            Insert.IntoTable("Users").Row(new User
            {
                UserId = Guid.NewGuid().ToString(),
                Email = "david@cumps.be",
                HashAndSalt = saltedHash.GetHashAndSaltString("admin"),
                DisplayName = "David Cumps"
            });
        }

        public override void Down()
        {
            Delete.Column("HashAndSalt").FromTable("Users");

            Alter.Table("Users")
                .AddColumn("Hash").AsFixedLengthString(64).NotNullable()
                .AddColumn("Salt").AsFixedLengthString(8).NotNullable();
        }
    }
}
