namespace CC.TheBench.Frontend.Web.Views.Account.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}