namespace CC.TheBench.Frontend.Web.Modules
{
    using Data;
    using Nancy;

    public class BaseModule : NancyModule
    {
        protected dynamic ReadStore;

        public BaseModule(IReadStoreFactory readStoreFactory)
        {
            ReadStore = readStoreFactory.ReadStore();
        }

        public BaseModule(IReadStoreFactory readStoreFactory, string modulePath) : base(modulePath)
        {
            ReadStore = readStoreFactory.ReadStore();
        }
    }
}