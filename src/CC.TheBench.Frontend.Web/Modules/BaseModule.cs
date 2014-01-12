namespace CC.TheBench.Frontend.Web.Modules
{
    using Data;
    using Nancy;
    using Security.Extensions;

    public abstract class BaseModule : NancyModule
    {
        private readonly IReadStoreFactory _readStoreFactory;

        private dynamic _readStore;

        protected dynamic ReadStore
        {
            get { return _readStore ?? (_readStore = _readStoreFactory.ReadStore()); }
        }

        protected bool IsAuthenticated
        {
            get { return this.IsAuthenticated(); }
        }

        protected BaseModule(IReadStoreFactory readStoreFactory)
        {
            _readStoreFactory = readStoreFactory;
        }

        protected BaseModule(IReadStoreFactory readStoreFactory, string modulePath) : base(modulePath)
        {
            _readStoreFactory = readStoreFactory;
        }
    }
}