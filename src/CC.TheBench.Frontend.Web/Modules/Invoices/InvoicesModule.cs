namespace CC.TheBench.Frontend.Web.Modules.Invoices
{
    public class InvoicesModule : SecureNancyModule
    {
        public InvoicesModule()
            : base("/invoices")
        {
            Get["/"] = x =>
            {
                return View["overview"];
            };
        }
    }
}