namespace CC.TheBench.Frontend.Web.Modules
{
    using Data;
    using Nancy.Security;

    public abstract class SecureNancyModule : BaseModule
    {
        protected SecureNancyModule(IReadStoreFactory readStoreFactory)
            : base(readStoreFactory)
        {
            this.RequiresAuthentication();
        }

        protected SecureNancyModule(IReadStoreFactory readStoreFactory, string modulePath) 
            : base(readStoreFactory, modulePath)
        {
            this.RequiresAuthentication();
        }
    }
}