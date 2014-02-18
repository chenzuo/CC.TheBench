namespace CC.TheBench.Frontend.Web.Configuration
{
    using Properties;

    internal class EncryptionConfiguration
    {
        private readonly SecuritySettings _securitySettings;

        public string EncryptionProviderSalt
        {
            get { return _securitySettings.EncryptionProviderSalt; }
        }

        public string EncryptionProviderPassphrase
        {
            get { return _securitySettings.EncryptionProviderPassphrase; }
        }

        public string HMacProviderSalt
        {
            get { return _securitySettings.HMacProviderSalt; }
        }

        public string HMacProviderPassphrase
        {
            get { return _securitySettings.HMacProviderPassphrase; }
        }

        public int SaltLength
        {
            get { return _securitySettings.SaltLength; }
        }

        public int HashLength
        {
            get { return _securitySettings.HashLength; }
        }

        public int NumberOfIterations
        {
            get { return _securitySettings.NumberOfIterations; }
        }

        public EncryptionConfiguration(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
        }
    }
}