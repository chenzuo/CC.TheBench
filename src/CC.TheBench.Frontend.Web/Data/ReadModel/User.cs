namespace CC.TheBench.Frontend.Web.Data.ReadModel
{
    using WindowsAzure.Table.Attributes;

    public sealed class User : UserIdentity
    {
        public User()
        {
            IdentityProvider = IdentityType.Manual.ToString();
        }

        [PartitionKey]
        public string Email { get; set; }
     
        public string HashAndSalt { get; set; }

        public string DisplayName { get; set; }
    }
}