namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public partial class TableStorageProvider
    {
        public void Insert<T>(string tableName, IEnumerable<CloudEntity<T>> entities)
        {
            foreach (var g in entities.GroupBy(e => e.PartitionKey))
                InsertInternal(tableName, g);
        }

        private void InsertInternal<T>(string tableName, IEnumerable<CloudEntity<T>> entities)
        {
            throw new NotImplementedException();
        } 
    }
}