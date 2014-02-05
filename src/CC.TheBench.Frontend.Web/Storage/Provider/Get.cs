namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Microsoft.WindowsAzure.Storage.Table;
    using Model;
    using Serialization;
    using Utilities;

    public partial class TableStorageProvider
    {
        public IEnumerable<CloudEntity<T>> Get<T>(string tableName)
        {
            if (tableName == null)
                throw new ArgumentNullException("tableName");

            return GetInternal<T>(tableName, string.Empty);
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey)
        {
            if (tableName == null)
                throw new ArgumentNullException("tableName");

            if (partitionKey == null)
                throw new ArgumentNullException("partitionKey");

            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            return GetInternal<T>(tableName, filter);
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey, IEnumerable<string> rowKeys)
        {
            if (tableName == null)
                throw new ArgumentNullException("tableName");

            if (partitionKey == null)
                throw new ArgumentNullException("partitionKey");

            foreach (var slice in Slice(rowKeys, MaxEntityTransactionCount))
            {
                var filter = new StringBuilder(string.Format("({0}) {1} (", 
                                                              TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                                                              TableOperators.And));

                for (var i = 0; i < slice.Length; i++)
                {
                    filter.AppendFormat("({0})",
                                         TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, slice[i]));

                    if (i < slice.Length - 1)
                        filter.AppendFormat(" {0} ", TableOperators.Or);
                }

                filter.Append(")");

                foreach (var entity in GetInternal<T>(tableName, filter.ToString()))
                {
                    yield return entity;
                }
            }
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey, string startRowKey, string endRowKey)
        {
            if (tableName == null)
                throw new ArgumentNullException("tableName");

            if (partitionKey == null)
                throw new ArgumentNullException("partitionKey");

            var filter = string.Format("({0})", TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            // optional starting range constraint
            if (!string.IsNullOrEmpty(startRowKey))
            {
                // ge = GreaterThanOrEqual (inclusive)
                filter += string.Format(" {0} ({1})", 
                                        TableOperators.And,
                                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, startRowKey));
            }

            if (!string.IsNullOrEmpty(endRowKey))
            {
                // lt = LessThan (exclusive)
                filter += string.Format(" {0} ({1})",
                                        TableOperators.And,
                                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, endRowKey)); 
            }

            return GetInternal<T>(tableName, filter);
        }

        private IEnumerable<CloudEntity<T>> GetInternal<T>(string tableName, string filter)
        {
            // CloudTable.ExecuteQuery handles continuation token internally
            // CloudTable.ExecuteQuerySegmented does not
            // http://stackoverflow.com/questions/16017001/does-azure-tablequery-handle-continuation-tokens-internally
            // We are using ExecuteQuerySegmented however since we want to start yielding results asap

            var table = _tableStorage.GetTableReference(tableName);
            var query = string.IsNullOrWhiteSpace(filter) ? new TableQuery<FatEntity>() : new TableQuery<FatEntity>().Where(filter);

            TableContinuationToken continuationToken = null;         

            do
            {
                TableQuerySegment<FatEntity> querySegment = null;
                FatEntity[] fatEntities = null;

                var token = continuationToken;
                Retry.Do(_policies.TransientTableErrorBackOff(), CancellationToken.None, () =>
                {
                    // TODO: Catch
                    //try
                    //{
                    querySegment = table.ExecuteQuerySegmented(query, token);
                    fatEntities = querySegment.Results.ToArray();
                    //}
                    //catch (DataServiceQueryException ex)
                    //{
                    //    // if the table does not exist, there is nothing to return
                    //    var errorCode = RetryPolicies.GetErrorCode(ex);
                    //    if (TableErrorCodeStrings.TableNotFound == errorCode
                    //        || StorageErrorCodeStrings.ResourceNotFound == errorCode)
                    //    {
                    //        fatEntities = new FatEntity[0];
                    //        return;
                    //    }

                    //    throw;
                    //}
                });

                foreach (var fatEntity in fatEntities)
                    yield return FatEntity.Convert<T>(fatEntity, _serializer);

                if (querySegment != null && querySegment.ContinuationToken != null)
                    continuationToken = querySegment.ContinuationToken;

            } while (continuationToken != null);
        }
    }
}