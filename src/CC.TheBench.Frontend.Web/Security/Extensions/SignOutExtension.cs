namespace CC.TheBench.Frontend.Web.Security.Extensions
{
    using System.Collections.Generic;
    using Microsoft.Owin;
    using Nancy;
    using Nancy.Owin;
    using Utilities.Extensions.DictionaryExtensions;

    public static class SignOutExtension
    {
        public static void SignOut(this NancyModule module)
        {
            var env = module.Context.Items.Get<IDictionary<string, object>>(NancyOwinHost.RequestEnvironmentKey);
            var owinContext = new OwinContext(env);

            owinContext.Authentication.SignOut(Constants.TheBenchAuthType);
        }
    }
}