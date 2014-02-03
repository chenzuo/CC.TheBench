namespace CC.TheBench.Frontend.Web.Data
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class CloudTable<T>
    {
        readonly ITableStorageProvider _provider;
        readonly string _tableName;

        public CloudTable(ITableStorageProvider provider)
        {
            var tableName = typeof(T).Name;
            // validating against the Windows Azure rule for table names.
            if (!Regex.Match(tableName, "^[A-Za-z][A-Za-z0-9]{2,62}").Success)
                throw new ArgumentException("Table name is incorrect", "tableName");

            _provider = provider;
            _tableName = tableName;
        }

        public CloudEntity<T> Get(string partitionName, string rowKey)
        {
            var entity = _provider.Get<T>(_tableName, partitionName, new[] { rowKey }).FirstOrDefault();
            return entity; //null != entity ? new Maybe<CloudEntity<T>>(entity) : Maybe<CloudEntity<T>>.Empty;
        }
    }
}