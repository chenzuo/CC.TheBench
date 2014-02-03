namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System;
    using System.Collections.Generic;
    using Model;

    public partial class TableStorageProvider
    {
        public IEnumerable<CloudEntity<T>> Get<T>(string tableName)
        {
            if (tableName == null)
                throw new ArgumentNullException("tableName");

            throw new NotImplementedException();
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey)
        {
            if (tableName == null)
                throw new ArgumentNullException("tableName");

            if (partitionKey == null)
                throw new ArgumentNullException("partitionKey");

            throw new NotImplementedException();
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey, IEnumerable<string> rowKeys)
        {
            if (tableName == null)
                throw new ArgumentNullException("tableName");

            if (partitionKey == null)
                throw new ArgumentNullException("partitionKey");

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
            return null;
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey, string startRowKey, string endRowKey)
        {
            if (tableName == null)
                throw new ArgumentNullException("tableName");

            if (partitionKey == null)
                throw new ArgumentNullException("partitionKey");

            throw new NotImplementedException();
        }
    }
}