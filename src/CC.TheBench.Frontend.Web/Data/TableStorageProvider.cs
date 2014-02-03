namespace CC.TheBench.Frontend.Web.Data
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>Implementation based on the Table Storage of Windows Azure.</summary>
    public class TableStorageProvider : ITableStorageProvider
    {
        private readonly CloudTableClient _tableStorage;
        private readonly IDataSerializer _serializer;

        public TableStorageProvider(CloudTableClient tableStorage)
        {
            //_policies = new RetryPolicies(observer);
            _tableStorage = tableStorage;
            _serializer = new CloudFormatter();
            //_observer = observer;
        }

        public IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey, IEnumerable<string> rowKeys)
        {
            if (null == tableName) 
                throw new ArgumentNullException("tableName");

            if (null == partitionKey) 
                throw new ArgumentNullException("partitionKey");

            // TODO: Finish http://www.windowsazure.com/en-us/documentation/articles/storage-dotnet-how-to-use-table-storage-20/?fb=nl-nl
            return null;
        }
    }
}