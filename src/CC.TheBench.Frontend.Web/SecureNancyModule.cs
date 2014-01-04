namespace CC.TheBench.Frontend.Web
{
    using Nancy;
    using Nancy.Security;

    public class SecureNancyModule : NancyModule
    {
        public SecureNancyModule()
        {
            this.RequiresAuthentication();
        }

        public SecureNancyModule(string modulePath) : base(modulePath)
        {
            this.RequiresAuthentication();
        }
    }
}