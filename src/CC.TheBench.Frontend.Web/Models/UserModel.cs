namespace CC.TheBench.Frontend.Web.Models
{
    public class UserModel
    {
        public string Email { get; private set; }

        public UserModel(string email)
        {
            Email = email;
        }
    }
}