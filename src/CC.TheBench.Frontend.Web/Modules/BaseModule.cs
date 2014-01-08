namespace CC.TheBench.Frontend.Web.Modules
{
    using Data;
    using Nancy;

    public class BaseModule : NancyModule
    {
        private readonly IReadStoreFactory _readStoreFactory;

        private dynamic _readStore;

        protected dynamic ReadStore
        {
            get { return _readStore ?? (_readStore = _readStoreFactory.ReadStore()); }
        }

        public BaseModule(IReadStoreFactory readStoreFactory)
        {
            _readStoreFactory = readStoreFactory;
        }

        public BaseModule(IReadStoreFactory readStoreFactory, string modulePath) : base(modulePath)
        {
            _readStoreFactory = readStoreFactory;
        }
    }
}