namespace CC.TheBench.Frontend.Web.Security
{
    using System;
    using System.Collections.Generic;
    using Nancy;
    using Nancy.Owin;
    using Nancy.Responses;

    public static class RequiresProductionHttpsExtension
    {
        /// <summary>
        /// This module requires https.
        /// </summary>
        /// <param name="module">The <see cref="INancyModule"/> that requires HTTPS.</param>
        public static void RequiresProductionHttps(this INancyModule module)
        {
            module.RequiresProductionHttps(true);
        }

        /// <summary>
        /// This module requires https.
        /// </summary>
        /// <param name="module">The <see cref="INancyModule"/> that requires HTTPS.</param>
        /// <param name="redirect"><see langword="true"/> if the user should be redirected to HTTPS (no port number) if the incoming request was made using HTTP, otherwise <see langword="false"/> if <see cref="HttpStatusCode.Forbidden"/> should be returned.</param>
        public static void RequiresProductionHttps(this INancyModule module, bool redirect)
        {
            module.Before.AddItemToEndOfPipeline(RequiresProductionHttps(redirect, null));
        }

        /// <summary>
        /// This module requires https.
        /// </summary>
        /// <param name="module">The <see cref="INancyModule"/> that requires HTTPS.</param>
        /// <param name="redirect"><see langword="true"/> if the user should be redirected to HTTPS if the incoming request was made using HTTP, otherwise <see langword="false"/> if <see cref="HttpStatusCode.Forbidden"/> should be returned.</param>
        /// <param name="httpsPort">The HTTPS port number to use</param>
        public static void RequiresProductionHttps(this INancyModule module, bool redirect, int httpsPort)
        {
            module.Before.AddItemToEndOfPipeline(RequiresProductionHttps(redirect, httpsPort));
        }

        private static Func<NancyContext, Response> RequiresProductionHttps(bool redirect, int? httpsPort)
        {
            return ctx =>
            {
                Response response = null;
                var request = ctx.Request;

                var env = (IDictionary<string, object>)ctx.Items[NancyOwinHost.RequestEnvironmentKey];

                var isLocal = env.ContainsKey("server.IsLocal") && (bool)env["server.IsLocal"];

                if (!request.Url.IsSecure && !isLocal)
                {
                    if (redirect && request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                    {
                        var redirectUrl = request.Url.Clone();
                        redirectUrl.Port = httpsPort;
                        redirectUrl.Scheme = "https";
                        response = new RedirectResponse(redirectUrl.ToString());
                    }
                    else
                    {
                        response = new Response { StatusCode = HttpStatusCode.Forbidden };
                    }
                }

                return response;
            };
        }
    }
}