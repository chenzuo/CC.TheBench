namespace CC.TheBench.Frontend.Web.Security.Extensions
{
    using System.Security.Claims;

    public static class GetClaimValueExtension
    {
        public static string GetClaimValue(this ClaimsPrincipal principal, string type)
        {
            var claim = principal.FindFirst(type);

            return claim != null 
                ? claim.Value 
                : null;
        } 
    }
}