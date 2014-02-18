namespace CC.TheBench.Frontend.Web.Security
{
    using System.Security.Claims;

    public static class TheBenchClaimTypes
    {
        public const string Email = ClaimTypes.Email;
        public const string Name = ClaimTypes.Name;

        public const string Role = ClaimTypes.Role;
    }
}