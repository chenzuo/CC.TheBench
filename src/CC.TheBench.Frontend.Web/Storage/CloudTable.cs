namespace CC.TheBench.Frontend.Web.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Provider;

    public class CloudTable<T>
    {
        private readonly ITableStorageProvider _provider;
        private readonly string _tableName;

        public string Name
        {
            get { return _tableName; }
        }

        public CloudTable(ITableStorageProvider provider)
        {
            _provider = provider;
            _tableName = typeof(T).Name;
        }

        public CloudEntity<T> Get(string partitionName, string rowKey)
        {
            var entity = _provider.Get<T>(_tableName, partitionName, new[] { rowKey }).FirstOrDefault();
            return entity;
        }

        public IEnumerable<CloudEntity<T>> Get()
        {
            return _provider.Get<T>(_tableName);
        }

        public IEnumerable<CloudEntity<T>> Get(string partitionKey)
        {
            return _provider.Get<T>(_tableName, partitionKey);
        }

        public IEnumerable<CloudEntity<T>> Get(string partitionKey, IEnumerable<string> rowKeys)
        {
            return _provider.Get<T>(_tableName, partitionKey, rowKeys);
        }

        public IEnumerable<CloudEntity<T>> Get(string partitionKey, string startRowKey, string endRowKey)
        {
            return _provider.Get<T>(_tableName, partitionKey, startRowKey, endRowKey);
        }

        public void Insert(IEnumerable<CloudEntity<T>> entities)
        {
            _provider.Insert(_tableName, entities);
        }

        public void Insert(CloudEntity<T> entity)
        {
            _provider.Insert(_tableName, new[] { entity });
        }

        public void Update(IEnumerable<CloudEntity<T>> entities)
        {
            _provider.Update(_tableName, entities, false);
        }

        public void Update(CloudEntity<T> entity)
        {
            _provider.Update(_tableName, new[] { entity }, false);
        }

        public void Upsert(IEnumerable<CloudEntity<T>> entities)
        {
            _provider.Upsert(_tableName, entities);
        }

        public void Upsert(CloudEntity<T> entity)
        {
            _provider.Upsert(_tableName, new[] { entity });
        }

        public void Delete(string partitionKey, IEnumerable<string> rowKeys)
        {
            _provider.Delete<T>(_tableName, partitionKey, rowKeys);
        }

        public void Delete(string partitionKey, string rowKey)
        {
            _provider.Delete<T>(_tableName, partitionKey, new[] { rowKey });
        }
    }
}