namespace CC.TheBench.Frontend.Web.Security.Extensions
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Data.ReadModel;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Nancy;
    using Nancy.Owin;
    using Utilities.Extensions.NancyExtensions;

    public static class SignInExtension
    {
        public static Response SignIn(this INancyModule module, IEnumerable<Claim> claims, bool isPersistent = false)
        {
            var env = module.Context.GetOwinEnvironment();
            var owinContext = new OwinContext(env);

            var identity = new ClaimsIdentity(claims, TheBenchConstants.TheBenchAuthType, TheBenchClaimTypes.Name, TheBenchClaimTypes.Role);
            var authOptions = new AuthenticationProperties { IsPersistent = isPersistent };

            owinContext.Authentication.SignIn(authOptions, identity);

            return module.AsRedirectQueryStringOrDefault("~/dashboard");
        }

        public static Response SignIn(this INancyModule module, User user, bool isPersistent = false)
        {
            var claims = new List<Claim>
            {
                new Claim(TheBenchClaimTypes.Email, user.Email),
                new Claim(TheBenchClaimTypes.Name, user.DisplayName)
            };

            return module.SignIn(claims, isPersistent);
        }
    }
}