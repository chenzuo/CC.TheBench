namespace CC.TheBench.Frontend.Web.Security.Extensions
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Data.ReadModel;
    using Microsoft.Owin;
    using Nancy;
    using Nancy.Owin;
    using Utilities.Extensions.DictionaryExtensions;
    using Utilities.Extensions.NancyExtensions;

    public static class SignInExtension
    {
        public static Response SignIn(this NancyModule module, IEnumerable<Claim> claims)
        {
            var env = module.Context.Items.Get<IDictionary<string, object>>(NancyOwinHost.RequestEnvironmentKey);
            var owinContext = new OwinContext(env);

            var identity = new ClaimsIdentity(claims, Constants.TheBenchAuthType);
            owinContext.Authentication.SignIn(identity);

            return module.AsRedirectQueryStringOrDefault("~/");
        }

        public static Response SignIn(this NancyModule module, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(TheBenchClaimTypes.Identifier, user.UserId),
                new Claim(TheBenchClaimTypes.Email, user.Email),
                new Claim(TheBenchClaimTypes.Name, user.DisplayName)
            };

            return module.SignIn(claims);
        }
    }
}