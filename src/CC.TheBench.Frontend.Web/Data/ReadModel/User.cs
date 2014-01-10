namespace CC.TheBench.Frontend.Web.Data.ReadModel
{
    public class User
    {
        public string UserId { get; set; }
        
        public string Email { get; set; }
        public string HashAndSalt { get; set; }

        public string DisplayName { get; set; }
    }
}