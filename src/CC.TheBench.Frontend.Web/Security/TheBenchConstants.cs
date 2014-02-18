namespace CC.TheBench.Frontend.Web.Security
{
    using System.Security.Claims;

    public static class TheBenchConstants
    {
        public static readonly TheBenchPrincipal AnonymousPrincipal = new TheBenchPrincipal(new ClaimsPrincipal());

        public static readonly string TheBenchAuthType = "TheBench";
    }
}