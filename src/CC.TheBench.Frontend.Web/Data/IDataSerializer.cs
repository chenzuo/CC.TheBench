namespace CC.TheBench.Frontend.Web.Data
{
    using System;
    using System.IO;

    /// <summary>
    /// Generic data serializer interface.
    /// </summary>
    public interface IDataSerializer
    {
        /// <summary>Serializes the object to the specified stream.</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="destinationStream">The destination stream.</param>
        /// <param name="type">The type of the object to serialize (can be a base type of the provided instance).</param>
        void Serialize(object instance, Stream destinationStream, Type type);

        /// <summary>Deserializes the object from specified source stream.</summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <returns>deserialized object</returns>
        object Deserialize(Stream sourceStream, Type type);
    }
}