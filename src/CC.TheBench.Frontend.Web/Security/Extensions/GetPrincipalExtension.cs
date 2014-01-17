namespace CC.TheBench.Frontend.Web.Security.Extensions
{
    using Nancy;
    using Nancy.ViewEngines.Razor;

    public static class GetPrincipalExtension
    {
        public static TheBenchPrincipal GetPrincipal<T>(this HtmlHelpers<T> htmlHelper)
        {
            var userIdentity = htmlHelper.CurrentUser as TheBenchUser;

            return userIdentity == null
                ? null
                : userIdentity.Principal;
        }

        public static TheBenchPrincipal GetPrincipal(this INancyModule module)
        {
            var userIdentity = module.Context.CurrentUser as TheBenchUser;

            return userIdentity == null 
                ? null 
                : userIdentity.Principal;
        }
    }
}