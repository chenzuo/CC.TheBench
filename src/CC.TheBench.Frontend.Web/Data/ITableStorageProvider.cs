namespace CC.TheBench.Frontend.Web.Data
{
    using System.Collections.Generic;

    /// <summary>Abstraction for the Table Storage.</summary>
    /// <remarks>This provider represents a logical abstraction of the Table Storage,
    /// not the Table Storage itself. In particular, implementations handle paging
    /// and query splitting internally. Also, this provider implicitly relies on
    /// serialization to handle generic entities (not constrained by the few datatypes
    /// available to the Table Storage).</remarks>
    public interface ITableStorageProvider
    {
        /// <summary>Iterates through all entities specified by their row keys.</summary>
        /// <param name="tableName">The name of the table. This table should exists otherwise the method will fail.</param>
        /// <param name="partitionKey">Partition key (can not be null).</param>
        /// <param name="rowKeys">lazy enumeration of non null string representing rowKeys.</param>
        /// <remarks>The enumeration is typically expected to be lazy, iterating through
        /// all the entities with paged request. If the table or the partition key does not exist,
        /// the returned enumeration is empty.</remarks>
        IEnumerable<CloudEntity<T>> Get<T>(string tableName, string partitionKey, IEnumerable<string> rowKeys);
    }
}