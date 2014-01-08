namespace CC.TheBench.Frontend.Web.Modules
{
    using Data;
    using Nancy;

    public class BaseModule : NancyModule
    {
        protected dynamic ReadStore;

        public IReadStoreFactory ReadStoreFactory { private get; set; }

        public BaseModule()
        {
            ReadStore = ReadStoreFactory.ReadStore();
        }

        public BaseModule(string modulePath) : base(modulePath)
        {
            ReadStore = ReadStoreFactory.ReadStore();
        }
    }
}