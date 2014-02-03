namespace CC.TheBench.Frontend.Web
{
    using System.Net;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Cryptography;
    using Nancy.Diagnostics;
    using Nancy.Owin;
    using Nancy.Security;
    using Nancy.TinyIoc;
    using Security;
    using Storage.Provider;
    using Utilities.Extensions.DictionaryExtensions;

    internal class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly TheBenchSettings _configuration;

        protected override CryptographyConfiguration CryptographyConfiguration
        {
            get
            {
                // This is to prevent app restarts from invalidating existing CSRF and Forms Cookies
                // If you are using the PassphraseKeyGenerator then it should be initialized at application startup 
                // because the algorithm is too slow to do per-request. This means that your salt will be static so 
                // the passphrase should be long and complex.
                return new CryptographyConfiguration(
                    new RijndaelEncryptionProvider(new PassphraseKeyGenerator(_configuration.Encryption.EncryptionProviderPassphrase,
                                                                              Encoding.ASCII.GetBytes(_configuration.Encryption.EncryptionProviderSalt))),
                    new DefaultHmacProvider(
                        new PassphraseKeyGenerator(_configuration.Encryption.HMacProviderPassphrase,
                                                   Encoding.ASCII.GetBytes(_configuration.Encryption.HMacProviderSalt))));
            }
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = _configuration.General.DiagnosticsConfigurationPassphrase }; }
        }

        public Bootstrapper(TheBenchSettings configuration)
        {
            _configuration = configuration;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            // Password hasher using PBKDF2
            var saltedHash = new SaltedHash(
                _configuration.Encryption.SaltLength,
                _configuration.Encryption.HashLength,
                _configuration.Encryption.NumberOfIterations);

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

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            // TODO: http://stackoverflow.com/questions/10813816/how-expensive-is-creation-of-cloudstorageaccount-or-cloudblobclient-instances-fr#10814400
            //       http://msdn.microsoft.com/en-us/library/microsoft.windowsazure.serviceruntime.roleenvironment.changing.aspx

            // CloudStorageAccount is safe to keep around for the entire application
            // Turn off Nagle: http://blogs.msdn.com/b/windowsazurestorage/archive/2010/06/25/nagle-s-algorithm-is-not-friendly-towards-small-requests.aspx
            var cloudStorageAccount = CloudStorageAccount.Parse(_configuration.Storage.AzureConnectionString);
            var tableServicePoint = ServicePointManager.FindServicePoint(cloudStorageAccount.TableEndpoint);
            tableServicePoint.UseNagleAlgorithm = false;

            container.Register(cloudStorageAccount);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            // Need a new CloudTableClient per request, it's not thread safe
            var tableClient = container.Resolve<CloudStorageAccount>().CreateCloudTableClient();
            tableClient.RetryPolicy = new NoRetry();

            container.Register<ITableStorageProvider>((c,p) => new TableStorageProvider(tableClient));
        }

        private static Response FlowPrincipal(NancyContext context)
        {
            var env = context.GetOwinEnvironment();
            if (env == null) 
                return null;

            var principal = env.Get<IPrincipal>("server.User") as ClaimsPrincipal;
            if (principal != null)
                context.CurrentUser = new TheBenchUser(new TheBenchPrincipal(principal));

            return null;
        }
    }
}