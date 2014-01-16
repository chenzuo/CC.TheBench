namespace CC.TheBench.Frontend.Web.Security
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Extensions;
    using Nancy.Security;

    public class TheBenchIdentity : IUserIdentity, ITheBenchIdentity
    {
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; private set; }

        public string Name
        {
            get { return ClaimsPrincipal.GetClaimValue(TheBenchClaimTypes.Name); }
        }

        public TheBenchIdentity(ClaimsPrincipal claimsPrincipal)
        {
            ClaimsPrincipal = claimsPrincipal;

            UserName = claimsPrincipal.GetClaimValue(TheBenchClaimTypes.Email);
            Claims = claimsPrincipal.FindAll(TheBenchClaimTypes.Role).Select(x => x.Value);
        }
    }

    public interface ITheBenchIdentity
    {
        string Name { get; }
    }
}