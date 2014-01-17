namespace CC.TheBench.Frontend.Web.Security
{
    using System.Security.Claims;
    using Extensions;

    public sealed class TheBenchPrincipal : ClaimsPrincipal
    {
        public string Name
        {
            get { return this.GetClaimValue(TheBenchClaimTypes.Name); }
        }

        public string Email
        {
            get { return this.GetClaimValue(TheBenchClaimTypes.Email); }
        }

        public TheBenchPrincipal(ClaimsPrincipal principal)
        {
            AddIdentities(principal.Identities);
        }
    }
}