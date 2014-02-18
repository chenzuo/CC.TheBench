namespace CC.TheBench.Frontend.Web.Configuration
{
    using Properties;

    internal class TheBenchSettings
    {
        public AuthenticationConfiguration Authentication { get; private set; }

        public EncryptionConfiguration Encryption { get; private set; }

        public GeneralConfiguration General { get; private set; }

        public StorageConfiguration Storage { get; private set; }

        public TheBenchSettings()
        {
            var securitySettings = SecuritySettings.Default;

            Authentication = new AuthenticationConfiguration(securitySettings);
            Encryption = new EncryptionConfiguration(securitySettings);
            General = new GeneralConfiguration(securitySettings);
            Storage = new StorageConfiguration();
        }
    }
}