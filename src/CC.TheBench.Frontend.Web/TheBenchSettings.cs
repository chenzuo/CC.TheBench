namespace CC.TheBench.Frontend.Web
{
    using System;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Nancy;
    using Properties;
    using Security;

    internal class TheBenchSettings
    {
        public TheBenchSettings()
        {
            var securitySettings = SecuritySettings.Default;
            Authentication = new Authentication(securitySettings);
            Encryption = new Encryption(securitySettings);
            General = new General(securitySettings);
        }

        public Authentication Authentication { get; private set; }

        public Encryption Encryption { get; private set; }

        public General General { get; private set; }
    }

    internal class General
    {
        private readonly SecuritySettings _securitySettings;

        public General(SecuritySettings securitySettings)
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

    internal class Authentication
    {
        private readonly SecuritySettings _securitySettings;

        public Authentication(SecuritySettings securitySettings)
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

    internal class Encryption
    {
        private readonly SecuritySettings _securitySettings;

        public Encryption(SecuritySettings securitySettings)
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
}