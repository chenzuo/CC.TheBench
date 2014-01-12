namespace CC.TheBench.Frontend.Web
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using Data;
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Conventions;
    using Nancy.Cryptography;
    using Nancy.Diagnostics;
    using Nancy.Owin;
    using Nancy.Security;
    using Nancy.TinyIoc;
    using Properties;
    using Security;
    using Utilities.Extensions.DictionaryExtensions;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override CryptographyConfiguration CryptographyConfiguration
        {
            get
            {
                // This is to prevent app restarts from invalidating existing CSRF and Forms Cookies
                // If you are using the PassphraseKeyGenerator then it should be initialized at application startup 
                // because the algorithm is too slow to do per-request. This means that your salt will be static so 
                // the passphrase should be long and complex.
                return new CryptographyConfiguration(
                    new RijndaelEncryptionProvider(new PassphraseKeyGenerator(SecuritySettings.Default.EncryptionProviderPassphrase,
                                                                              Encoding.ASCII.GetBytes(SecuritySettings.Default.EncryptionProviderSalt))),
                    new DefaultHmacProvider(
                        new PassphraseKeyGenerator(SecuritySettings.Default.HMacProviderPassphrase,
                                                   Encoding.ASCII.GetBytes(SecuritySettings.Default.HMacProviderSalt))));
            }
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = SecuritySettings.Default.DiagnosticsConfigurationPassphrase }; }
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            // Password hasher using PBKDF2
            var saltedHash = new SaltedHash(
                SecuritySettings.Default.SaltLength,
                SecuritySettings.Default.HashLength,
                SecuritySettings.Default.NumberOfIterations);

            container.Register<ISaltedHash, SaltedHash>(saltedHash);
            
            // Disable _Nancy
            DiagnosticsHook.Disable(pipelines);

            // Takes care of outputting the CSRF token to Cookies
            Csrf.Enable(pipelines);

            // Don't show errors when we are in release
            StaticConfiguration.DisableErrorTraces = !StaticConfiguration.IsRunningDebug;

            // Wire up flowing of OWIN user to a Nancy one
            pipelines.BeforeRequest.AddItemToStartOfPipeline(FlowPrincipal);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            // Simple.Data is quite aggressive in closing connections and holds no open connections to a data store by default, 
            // so you can keep the Database object returned from the Open*() methods hanging around without worrying.
            container.Register<IReadStoreFactory, ReadStoreFactory>().AsSingleton();
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/css", @"/Public/Styles"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/js", @"/Public/Scripts"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/fonts", @"/Public/Fonts"));
        }

        private static Response FlowPrincipal(NancyContext context)
        {
            // TODO: Replace with context.GetOwinEnvironment() when v0.22 is released
            var env = context.Items.Get<IDictionary<string, object>>(NancyOwinHost.RequestEnvironmentKey);
            if (env == null) 
                return null;

            var principal = env.Get<IPrincipal>("server.User") as ClaimsPrincipal;
            if (principal != null)
                context.CurrentUser = new TheBenchIdentity(principal);

            return null;
        }
    }
}