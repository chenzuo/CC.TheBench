namespace CC.TheBench.Frontend.Web.Configuration
{
    using Nancy;
    using Properties;

    internal class GeneralConfiguration
    {
        private readonly SecuritySettings _securitySettings;

        public string DiagnosticsConfigurationPassphrase
        {
            get { return _securitySettings.DiagnosticsConfigurationPassphrase; }
        }

        public bool RequireHttps
        {
            get { return !StaticConfiguration.IsRunningDebug; }
        }

        public GeneralConfiguration(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
        }
    }
}