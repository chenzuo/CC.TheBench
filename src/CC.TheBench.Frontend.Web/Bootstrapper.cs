namespace CC.TheBench.Frontend.Web
{
    using System.Text;
    using Data;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Bootstrapper;
    using Nancy.Conventions;
    using Nancy.Cryptography;
    using Nancy.Diagnostics;
    using Nancy.Security;
    using Nancy.TinyIoc;
    using Properties;
    using Security;
    using Utilities;

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

            // Stateless utilities
            container.Register<ISaltedHash, SaltedHash>().AsSingleton();
            
            // Disable _Nancy
            DiagnosticsHook.Disable(pipelines);

            // Takes care of outputting the CSRF token to Cookies
            Csrf.Enable(pipelines);

            StaticConfiguration.DisableErrorTraces = !StaticConfiguration.IsRunningDebug;
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            // Simple.Data is quite aggressive in closing connections and holds no open connections to a data store by default, 
            // so you can keep the Database object returned from the Open*() methods hanging around without worrying.
            container.Register<IReadStoreFactory, ReadStoreFactory>().AsSingleton();

            // Here we register our user mapper as a per-request singleton.
            // As this is now per-request we could inject a request scoped
            // database "context" or other request scoped services.
            container.Register<IUserMapper, UserMapper>();
        }

        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            // Turning on Forms Authentication - for this request
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration
                {
                    CryptographyConfiguration = requestContainer.Resolve<CryptographyConfiguration>(),
                    RedirectUrl = "~/account/login",
                    UserMapper = requestContainer.Resolve<IUserMapper>(),
                    RequiresSSL = !StaticConfiguration.IsRunningDebug
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/css", @"/Public/Styles"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/js", @"/Public/Scripts"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/fonts", @"/Public/Fonts"));
        }
    }
}