namespace CC.TheBench.Frontend.Web.Configuration
{
    using System.Configuration;

    internal class StorageConfiguration
    {
        public string AzureConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["TheBenchTableStorage"].ConnectionString; }
        }
    }
}