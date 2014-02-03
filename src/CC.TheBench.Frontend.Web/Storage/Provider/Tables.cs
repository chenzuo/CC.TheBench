namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Utilities;

    public partial class TableStorageProvider
    {
        public bool CreateTable(string tableName)
        {
            var flag = false;

            Retry.Do(_policies.SlowInstantiation(), CancellationToken.None, () =>
            {
                var table = _tableStorage.GetTableReference(tableName);
                flag = table.CreateIfNotExists();
            });

            return flag;
        }

        public bool DeleteTable(string tableName)
        {
            var flag = false;

            Retry.Do(_policies.SlowInstantiation(), CancellationToken.None, () =>
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