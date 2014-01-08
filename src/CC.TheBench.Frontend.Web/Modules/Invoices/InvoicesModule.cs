namespace CC.TheBench.Frontend.Web.Modules.Invoices
{
    using Data;
    using Models;

    public class InvoicesModule : SecureNancyModule
    {
        public InvoicesModule(IReadStoreFactory readStoreFactory)
            : base(readStoreFactory, "/invoices")
        {
            Get["/"] = x =>
            {
                var model = new UserModel(Context.CurrentUser.UserName);
                return View["overview", model];
            };
        }
    }
}