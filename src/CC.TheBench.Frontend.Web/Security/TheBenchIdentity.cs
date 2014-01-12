namespace CC.TheBench.Frontend.Web.Security
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Extensions;
    using Nancy.Security;

    public class TheBenchIdentity : IUserIdentity
    {
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; private set; }

        public TheBenchIdentity(ClaimsPrincipal claimsPrincipal)
        {
            ClaimsPrincipal = claimsPrincipal;

            UserName = claimsPrincipal.GetClaimValue(TheBenchClaimTypes.Email);
            Claims = claimsPrincipal.FindAll(TheBenchClaimTypes.Role).Select(x => x.Value);
        }
    }
}