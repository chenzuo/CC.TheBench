namespace CC.TheBench.Frontend.Web.Utilities.Extensions.NancyExtensions
{
    using Nancy;

    public static class AsRedirectQueryStringOrDefaultExtension
    {
        public static Response AsRedirectQueryStringOrDefault(this NancyModule module, string defaultUrl)
        {
            string returnUrl = module.Request.Query.returnUrl;

            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = defaultUrl;

            return module.Response.AsRedirect(returnUrl);
        }
    }
}