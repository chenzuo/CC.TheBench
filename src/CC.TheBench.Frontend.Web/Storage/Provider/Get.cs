namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.WindowsAzure.Storage.Table;
    using Model;

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
            // http://stackoverflow.com/questions/16017001/does-azure-tablequery-handle-continuation-tokens-internally

            //var query = new TableQuery<T>();
            //var query = new TableQuery<T>().Where(filter);
            // table.ExecuteQuery(rangeQuery)
            /* var table = _tableStorage.GetTableReference(tableName);
             var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKeys.ElementAt(0));
             var retrievedResult = table.Execute(retrieveOperation);

             var result = new List<CloudEntity<T>>(1);
             if (retrievedResult.Result != null)
                 result.Add(new CloudEntity<T>
                 {
                     Value = retrievedResult.Result as T
                 });

             return result;*/

            throw new NotImplementedException();
        }
    }
}