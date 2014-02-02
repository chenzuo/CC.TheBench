namespace CC.TheBench.Frontend.Web.Modules.Dashboard
{
    using Views.Dashboard.Models;

    public class OverviewModule : SecureNancyModule
    {
        public OverviewModule()
            : base("/dashboard")
        {
            Get["/"] = x =>
            {
                return View["overview", new OverviewModel()];
            };
        }
    }
}