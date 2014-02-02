namespace CC.TheBench.Frontend.Web.Modules
{
    using Nancy.Security;

    public abstract class SecureNancyModule : BaseModule
    {
        protected SecureNancyModule()
        {
            this.RequiresAuthentication();
        }

        protected SecureNancyModule(string modulePath) 
            : base(modulePath)
        {
            this.RequiresAuthentication();
        }
    }
}