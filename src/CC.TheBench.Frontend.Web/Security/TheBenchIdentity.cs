namespace CC.TheBench.Frontend.Web.Security
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Nancy.Security;

    public class TheBenchIdentity : IUserIdentity
    {
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; private set; }

        public TheBenchIdentity(ClaimsPrincipal claimsPrincipal)
        {
            ClaimsPrincipal = claimsPrincipal;

            UserName = claimsPrincipal.FindFirst(TheBenchClaimTypes.Email).Value;
        }
    }
}