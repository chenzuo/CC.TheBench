namespace CC.TheBench.Frontend.Web.Configuration
{
    using System;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Nancy;
    using Properties;
    using Security;

    internal class AuthenticationConfiguration
    {
        private readonly SecuritySettings _securitySettings;

        public string AuthenticationType
        {
            get { return TheBenchConstants.TheBenchAuthType; }
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
                    ? CookieSecureOption.SameAsRequest
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

        public AuthenticationConfiguration(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
        }
    }
}