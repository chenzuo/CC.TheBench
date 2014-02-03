namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public partial class TableStorageProvider
    {
        public void Upsert<T>(string tableName, IEnumerable<CloudEntity<T>> entities)
        {
            foreach (var g in entities.GroupBy(e => e.PartitionKey))
                UpsertInternal(tableName, g);
        }

        private void UpsertInternal<T>(string tableName, IEnumerable<CloudEntity<T>> entities)
        {
            throw new NotImplementedException();
        }
    }
}