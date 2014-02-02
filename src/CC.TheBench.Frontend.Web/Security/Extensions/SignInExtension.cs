﻿namespace CC.TheBench.Frontend.Web.Security.Extensions
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Data.ReadModel;
    using Microsoft.Owin;
    using Nancy;
    using Nancy.Owin;
    using Utilities.Extensions.NancyExtensions;

    public static class SignInExtension
    {
        public static Response SignIn(this INancyModule module, IEnumerable<Claim> claims)
        {
            var env = module.Context.GetOwinEnvironment();
            var owinContext = new OwinContext(env);

            var identity = new ClaimsIdentity(claims, Constants.TheBenchAuthType);
            owinContext.Authentication.SignIn(identity);

            return module.AsRedirectQueryStringOrDefault("~/dashboard");
        }

        public static Response SignIn(this INancyModule module, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(TheBenchClaimTypes.Identifier, user.Email), // TODO: Double check
                new Claim(TheBenchClaimTypes.Email, user.Email),
                new Claim(TheBenchClaimTypes.Name, user.DisplayName)
            };

            return module.SignIn(claims);
        }
    }
}