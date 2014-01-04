namespace CC.TheBench.Frontend.Web
{
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // https://github.com/NancyFx/Nancy/wiki/Hosting-nancy-with-owin
            app.UseNancy();
        }
    }
}