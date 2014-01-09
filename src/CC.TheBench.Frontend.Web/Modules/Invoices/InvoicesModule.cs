namespace CC.TheBench.Frontend.Web.Modules.Invoices
{
    using Data;

    public class InvoicesModule : SecureNancyModule
    {
        public InvoicesModule(IReadStoreFactory readStoreFactory)
            : base(readStoreFactory, "/invoices")
        {
            Get["/"] = x =>
            {
                return View["overview"];
            };
        }
    }
}