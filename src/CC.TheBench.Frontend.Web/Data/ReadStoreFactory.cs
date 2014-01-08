namespace CC.TheBench.Frontend.Web.Data
{
    public class ReadStoreFactory : IReadStoreFactory
    {
        private dynamic _db;

        public dynamic ReadStore()
        {
            // TODO: Open DB from Named connection
            return _db ?? (_db = Simple.Data.Database.Open());
        }
    }
}