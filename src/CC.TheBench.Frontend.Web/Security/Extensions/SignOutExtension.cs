namespace CC.TheBench.Frontend.Web.Security.Extensions
{
    using Microsoft.Owin;
    using Nancy;
    using Nancy.Owin;

    public static class SignOutExtension
    {
        public static void SignOut(this NancyModule module)
        {
            var env = module.Context.GetOwinEnvironment();
            var owinContext = new OwinContext(env);
            
            owinContext.Authentication.SignOut(TheBenchConstants.TheBenchAuthType);
        }
    }
}