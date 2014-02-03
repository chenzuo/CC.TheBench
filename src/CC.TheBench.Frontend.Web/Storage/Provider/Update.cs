namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public partial class TableStorageProvider
    {
        public void Update<T>(string tableName, IEnumerable<CloudEntity<T>> entities, bool force)
        {
            foreach (var g in entities.GroupBy(e => e.PartitionKey))
                UpdateInternal(tableName, g, force);
        }

        private void UpdateInternal<T>(string tableName, IEnumerable<CloudEntity<T>> entities, bool force)
        {
            throw new NotImplementedException();
        }
    }
}