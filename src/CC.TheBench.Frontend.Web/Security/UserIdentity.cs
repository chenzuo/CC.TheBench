namespace CC.TheBench.Frontend.Web.Security
{
    using System.Collections.Generic;
    using Nancy.Security;

    public class UserIdentity : IUserIdentity
    {
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }

        public string Email { get; set; }
    }
}