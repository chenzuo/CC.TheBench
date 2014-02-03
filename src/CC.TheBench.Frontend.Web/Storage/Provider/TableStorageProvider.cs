﻿namespace CC.TheBench.Frontend.Web.Storage.Provider
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.Storage.Table;
    using Serialization;
    using Utilities;

    /// <summary>Implementation based on the Table Storage of Windows Azure.</summary>
    public partial class TableStorageProvider : ITableStorageProvider
    {
        // HACK: those tokens will probably be provided as constants in the StorageClient library
        const int MaxEntityTransactionCount = 100;

        // HACK: Lowering the maximal payload, to avoid corner cases #117 (ContentLengthExceeded)
        // [vermorel] 128kB is purely arbitrary, only taken as a reasonable safety margin
        const int MaxEntityTransactionPayload = 4 * 1024 * 1024 - 128 * 1024; // 4 MB - 128kB

        private readonly CloudTableClient _tableStorage;
        private readonly IDataSerializer _serializer;
        private readonly RetryPolicies _policies;

        private CloudTableClient AzureClient
        {
            get { return _tableStorage; }
        }

        public TableStorageProvider(CloudTableClient tableStorage)
        {
            _policies = new RetryPolicies();
            _tableStorage = tableStorage;
            _serializer = new CloudFormatter();
        }

        private CloudTable GetTable(string tableName)
        {
            if (tableName == null)
                throw new ArgumentNullException("tableName");

            return AzureClient.GetTableReference(tableName);
        }

        /// <summary>Slice entities according the payload limitation of
        /// the transaction group, plus the maximal number of entities to
        /// be embedded into a single transaction.</summary>
        private static IEnumerable<T[]> SliceEntities<T>(IEnumerable<T> entities, Func<T, int> getPayload)
        {
            var accumulator = new List<T>(MaxEntityTransactionCount);
            var payload = 0;
            foreach (var entity in entities)
            {
                var entityPayLoad = getPayload(entity);

                if (accumulator.Count >= MaxEntityTransactionCount || payload + entityPayLoad >= MaxEntityTransactionPayload)
                {
                    yield return accumulator.ToArray();
                    accumulator.Clear();
                    payload = 0;
                }

                accumulator.Add(entity);
                payload += entityPayLoad;
            }

            if (accumulator.Count > 0)
                yield return accumulator.ToArray();
        }
    }
}