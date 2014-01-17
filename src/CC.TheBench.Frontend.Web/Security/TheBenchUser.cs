namespace CC.TheBench.Frontend.Web.Security
{
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Nancy.Security;

    public class TheBenchUser : IUserIdentity
    {
        // Required by Nancy
        public string UserName
        {
            get { return Principal.GetClaimValue(TheBenchClaimTypes.Email); }
        }

        // Required by Nancy
        public IEnumerable<string> Claims
        {
            get { return Principal.FindAll(TheBenchClaimTypes.Role).Select(x => x.Value); }
        }

        public TheBenchPrincipal Principal { get; private set; }

        public TheBenchUser(TheBenchPrincipal principal)
        {
            Principal = principal;
        }
    }
}