namespace CC.TheBench.Frontend.Web.Modules.Invoices
{
    using Models;

    public class InvoicesModule : SecureNancyModule
    {
        public InvoicesModule() : base("/invoices")
        {
            Get["/"] = x =>
            {
                var model = new UserModel(Context.CurrentUser.UserName);
                return View["overview", model];
            };
        }
    }
}