namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System;
    using System.Collections.Generic;
    using Model;

    public partial class TableStorageProvider
    {
        public void Delete<T>(string tableName, string partitionKey, IEnumerable<string> rowKeys)
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(string tableName, IEnumerable<CloudEntity<T>> entities, bool force)
        {
            throw new NotImplementedException();
        }
    }
}