namespace CC.TheBench.Frontend.Web
{
    using Microsoft.Owin.Extensions;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // https://github.com/NancyFx/Nancy/wiki/Hosting-nancy-with-owin
            app.UseNancy();

            // http://katanaproject.codeplex.com/discussions/470920
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}