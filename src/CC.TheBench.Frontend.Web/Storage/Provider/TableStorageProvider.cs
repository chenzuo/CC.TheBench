namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Microsoft.WindowsAzure.Storage.Table;
    using Serialization;
    using Utilities;

    /// <summary>Implementation based on the Table Storage of Windows Azure.</summary>
    public partial class TableStorageProvider : ITableStorageProvider
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
    }
}