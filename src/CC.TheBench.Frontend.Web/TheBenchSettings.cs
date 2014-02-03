namespace CC.TheBench.Frontend.Web
{
    using System;
    using System.Configuration;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Nancy;
    using Properties;
    using Security;

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

    internal class GeneralConfiguration
    {
        private readonly SecuritySettings _securitySettings;

        public GeneralConfiguration(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
        }

        public string DiagnosticsConfigurationPassphrase
        {
            get { return _securitySettings.DiagnosticsConfigurationPassphrase; }
        }

        public bool RequireHttps
        {
            get { return !StaticConfiguration.IsRunningDebug; }
        }
    }

    internal class AuthenticationConfiguration
    {
        private readonly SecuritySettings _securitySettings;

        public AuthenticationConfiguration(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
        }

        public string AuthenticationType
        {
            get { return Constants.TheBenchAuthType; }
        }

        public TimeSpan AuthenticationCookieExpireTimeSpan
        {
            get { return TimeSpan.FromDays(_securitySettings.AuthenticationCookieExpirationInDays); }
        }

        public string CookieName
        {
            get { return "thebench.id"; }
        }

        public CookieSecureOption CookieSecure
        {
            get
            {
                return StaticConfiguration.IsRunningDebug
                    ? CookieSecureOption.Never
                    : CookieSecureOption.Always;
            }
        }

        public bool CookieHttpOnly
        {
            get { return true; }
        }

        public PathString LoginPath
        {
            get { return new PathString("/account/login"); }
        }

        public PathString LogoutPath
        {
            get { return new PathString("/account/logout"); }
        }

        public bool EnableSlidingExpiration
        {
            get { return true; }
        }

        public string ReturnUrl
        {
            get { return "returnUrl"; }
        }
    }

    internal class EncryptionConfiguration
    {
        private readonly SecuritySettings _securitySettings;

        public EncryptionConfiguration(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
        }

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
    }

    internal class StorageConfiguration
    {
        public string AzureConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["TheBenchTableStorage"].ConnectionString; }
        }
    }

}