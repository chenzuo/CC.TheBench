namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Microsoft.WindowsAzure.Storage.Table;
    using Model;
    using Serialization;
    using Utilities;

    public partial class TableStorageProvider
    {
        public void Insert<T>(string tableName, CloudEntity<T> entity)
        {
            InsertInternal(tableName, new[] { entity });
        }

        public void Insert<T>(string tableName, IEnumerable<CloudEntity<T>> entities)
        {
            foreach (var g in entities.GroupBy(e => e.PartitionKey))
                InsertInternal(tableName, g);
        }

        private void InsertInternal<T>(string tableName, IEnumerable<CloudEntity<T>> entities)
        {
            var table = GetTable(tableName);

            var fatEntities = entities.Select(e => Tuple.Create(FatEntity.Convert(e, _serializer), e));

            //var noBatchMode = false;

            foreach (var slice in SliceEntities(fatEntities, e => e.Item1.GetPayload()))
            {
                var batchOperation = new TableBatchOperation();

                var cloudEntityOfFatEntity = new Dictionary<object, CloudEntity<T>>();
                foreach (var fatEntity in slice)
                {
                    batchOperation.Insert(fatEntity.Item1);
                    cloudEntityOfFatEntity.Add(fatEntity.Item1, fatEntity.Item2);
                }

                Retry.Do(_policies.TransientTableErrorBackOff(), CancellationToken.None, () =>
                {
                    try
                    {
                        table.ExecuteBatch(batchOperation);
                    }
                    catch
                    {
                        // TODO: Implement
                    }
                });

                //batchOperation.Insert(customer1);
                //table.ExecuteBatch(batchOperation);
            }
        } 
    }
}