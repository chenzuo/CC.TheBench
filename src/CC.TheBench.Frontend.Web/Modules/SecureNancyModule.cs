namespace CC.TheBench.Frontend.Web.Modules
{
    using Data;
    using Nancy.Security;

    public class SecureNancyModule : BaseModule
    {
        public SecureNancyModule(IReadStoreFactory readStoreFactory)
            : base(readStoreFactory)
        {
            this.RequiresAuthentication();
        }

        public SecureNancyModule(IReadStoreFactory readStoreFactory, string modulePath) 
            : base(readStoreFactory, modulePath)
        {
            this.RequiresAuthentication();
        }
    }
}