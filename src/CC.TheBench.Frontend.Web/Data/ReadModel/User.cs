namespace CC.TheBench.Frontend.Web.Data.ReadModel
{
    using System;

    public class User
    {
        public Guid UserId { get; set; }
        
        public string Email { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }

        public string DisplayName { get; set; }
    }
}