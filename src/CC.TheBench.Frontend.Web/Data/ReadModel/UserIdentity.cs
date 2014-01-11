namespace CC.TheBench.Frontend.Web.Data.ReadModel
{
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
}