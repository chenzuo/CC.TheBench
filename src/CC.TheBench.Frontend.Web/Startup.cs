namespace CC.TheBench.Frontend.Web
{
    using Microsoft.Owin;
    using Microsoft.Owin.Extensions;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using Security;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // So that squishit works
            //Directory.SetCurrentDirectory(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

            //var ticketDataFormat = new TicketDataFormat(kernel.Get<IDataProtector>());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.TheBenchAuthType,
                CookieHttpOnly = true,
                CookieName = "thebench.id",
                LoginPath = new PathString("/account/login"),
                LogoutPath = new PathString("/account/logout"),
                //ExpireTimeSpan = TimeSpan.FromDays(30),
                //TicketDataFormat = ticketDataFormat,
                //Provider = new JabbRFormsAuthenticationProvider()
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