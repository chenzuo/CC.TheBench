namespace CC.TheBench.Frontend.Web.Modules
{
    using Nancy.Security;

    public class SecureNancyModule : BaseModule
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