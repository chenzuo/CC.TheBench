namespace CC.TheBench.Frontend.Web.Data.ReadModel
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class UserIdentity
    {
        public string UserId { get; set; }

        public string Provider { get; set; }
        public string Id { get; set; }

        public string Email { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Username { get; set; }
    }

    //public class UserIdentity : TableEntity
    //{

    //    public string HashAndSalt { get; set; }
    //    public string DisplayName { get; set; }

    //    public string Email { get; set; }
    //    public string Locale { get; set; }
    //    public string Name { get; set; }
    //    public string Picture { get; set; }
    //    public string Username { get; set; }
    //}
}
//PartitionKey: id (could also be email)
//Rowkey: type (facebook, twitter, manual)