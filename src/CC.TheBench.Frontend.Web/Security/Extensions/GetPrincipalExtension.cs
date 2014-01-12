namespace CC.TheBench.Frontend.Web.Security.Extensions
{
    using System.Security.Claims;
    using Nancy;

    public static class GetPrincipalExtension
    {
        public static ClaimsPrincipal GetPrincipal(this NancyModule module)
        {
            var userIdentity = module.Context.CurrentUser as TheBenchIdentity;

            return userIdentity == null 
                ? null 
                : userIdentity.ClaimsPrincipal;
        }
    }
}