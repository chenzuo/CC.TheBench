namespace CC.TheBench.Frontend.Web
{
    using System;
    using Microsoft.Owin;
    using Microsoft.Owin.Extensions;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.DataHandler;
    using Nancy;
    using Owin;
    using Properties;
    using Security;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // So that squishit works
            //Directory.SetCurrentDirectory(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.TheBenchAuthType,
                CookieHttpOnly = true,
                CookieName = "thebench.id",
                CookieSecure = StaticConfiguration.IsRunningDebug ? CookieSecureOption.Never : CookieSecureOption.Always,
                LoginPath = new PathString("/account/login"),
                LogoutPath = new PathString("/account/logout"),
                ExpireTimeSpan = TimeSpan.FromDays(SecuritySettings.Default.AuthenticationCookieExpirationInDays),
                SlidingExpiration = true,
                ReturnUrlParameter = "returnUrl"
            });

            // TODO: When Microsoft.Owin.StaticFiles gets out of pre-release, try that
            // Also try to pre-bundle resources and eventually embed them since it can serve that too
            // app.Map("/js", jsApp => jsApp.UseFileServer("/Public/Scripts"));

            // https://github.com/NancyFx/Nancy/wiki/Hosting-nancy-with-owin
            app.UseNancy();

            // http://katanaproject.codeplex.com/discussions/470920
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}