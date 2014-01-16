namespace CC.TheBench.Frontend.Web.Security.Extensions
{
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using Nancy;

    public static class IsAuthenticatedExtension
    {
        public static bool IsAuthenticated(this INancyModule module)
        {
            return module.GetPrincipal().IsAuthenticated();
        }

        public static bool IsAuthenticated(this IPrincipal principal)
        {
            if (principal == null)
                return false;

            var claimsPrincipal = principal as ClaimsPrincipal;

            if (claimsPrincipal == null)
                return false;

            var theBenchIdentity = claimsPrincipal.Identities
                                                  .FirstOrDefault(x => x.AuthenticationType == Constants.TheBenchAuthType);

            if (theBenchIdentity == null)
                return false;

            var idClaim = theBenchIdentity.FindFirst(TheBenchClaimTypes.Identifier);

            return idClaim != null;
        }
    }
}