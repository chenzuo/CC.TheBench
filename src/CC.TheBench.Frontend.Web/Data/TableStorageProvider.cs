namespace CC.TheBench.Frontend.Web.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>Implementation based on the Table Storage of Windows Azure.</summary>
    public class TableStorageProvider : ITableStorageProvider
    {
        private readonly CloudTableClient _tableStorage;
        private readonly IDataSerializer _serializer;
        private readonly RetryPolicies _policies;

        public TableStorageProvider(CloudTableClient tableStorage)
        {
            _policies = new RetryPolicies();
            _tableStorage = tableStorage;
            _serializer = new CloudFormatter();
            //_observer = observer;
        }

        public bool CreateTable(string tableName)
        {
            var flag = false;

            Retry.Do(_policies.SlowInstantiation(), CancellationToken.None,
                    () =>
                    {
                        var table = _tableStorage.GetTableReference(tableName);
                        flag = table.CreateIfNotExists();
                    });

            return flag;
        }

        public bool DeleteTable(string tableName)
        {
            var flag = false;

            Retry.Do(_policies.SlowInstantiation(), CancellationToken.None,
                  () =>
                  {
                      var table = _tableStorage.GetTableReference(tableName);
                      flag = table.DeleteIfExists();
                  });

            return flag;
        }

        public IEnumerable<string> GetTables()
        {
            return _tableStorage.ListTables().Select(x => x.Name);
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey, IEnumerable<string> rowKeys)
        {
            if (null == tableName) 
                throw new ArgumentNullException("tableName");

            if (null == partitionKey) 
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
            throw new NotImplementedException();
        }

        public void Insert<T>(string tableName, IEnumerable<CloudEntity<T>> entities)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(string tableName, IEnumerable<CloudEntity<T>> entities, bool force)
        {
            throw new NotImplementedException();
        }

        public void Upsert<T>(string tableName, IEnumerable<CloudEntity<T>> entities)
        {
            throw new NotImplementedException();
        }

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