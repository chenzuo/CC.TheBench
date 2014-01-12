using CC.TheBench.Frontend.Web;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup), "Configuration")]

namespace CC.TheBench.Frontend.Web
{
    using Microsoft.Owin.Extensions;
    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.StaticFiles;
    using Nancy;
    using Nancy.Owin;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // So that squishit works
            //Directory.SetCurrentDirectory(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

            var configuration = new TheBenchSettings();

            ConfigureAuthentication(app, configuration);
            ConfigureStaticContent(app);
            ConfigureNancy(app, configuration);

            // http://katanaproject.codeplex.com/discussions/470920
            app.UseStageMarker(PipelineStage.MapHandler);
        }

        private static void ConfigureAuthentication(IAppBuilder app, TheBenchSettings configuration)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = configuration.Authentication.AuthenticationType,
                CookieHttpOnly = configuration.Authentication.CookieHttpOnly,
                CookieName = configuration.Authentication.CookieName,
                CookieSecure = configuration.Authentication.CookieSecure,
                LoginPath = configuration.Authentication.LoginPath,
                LogoutPath = configuration.Authentication.LogoutPath,
                ExpireTimeSpan = configuration.Authentication.AuthenticationCookieExpireTimeSpan,
                SlidingExpiration = configuration.Authentication.EnableSlidingExpiration,
                ReturnUrlParameter = configuration.Authentication.ReturnUrl
            });
        }

        private static void ConfigureStaticContent(IAppBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString("/css"),
                FileSystem = new PhysicalFileSystem("Public/Styles")
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString("/js"),
                FileSystem = new PhysicalFileSystem("Public/Scripts")
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString("/fonts"),
                FileSystem = new PhysicalFileSystem("Public/Fonts")
            });
        }

        private static void ConfigureNancy(IAppBuilder app, TheBenchSettings configuration)
        {
            // https://github.com/NancyFx/Nancy/wiki/Hosting-nancy-with-owin
            app.UseNancy(new NancyOptions
            {
                Bootstrapper = new Bootstrapper(configuration),
            });
        }
    }
}