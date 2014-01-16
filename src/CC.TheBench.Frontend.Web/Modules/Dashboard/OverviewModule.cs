namespace CC.TheBench.Frontend.Web.Modules.Dashboard
{
    using Data;
    using Views.Dashboard.Models;

    public class OverviewModule : SecureNancyModule
    {
        public OverviewModule(IReadStoreFactory readStoreFactory)
            : base(readStoreFactory, "/dashboard")
        {
            Get["/"] = x =>
            {
                return View["overview", new OverviewModel()];
            };
        }
    }
}