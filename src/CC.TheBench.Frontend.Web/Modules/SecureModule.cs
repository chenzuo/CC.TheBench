namespace CC.TheBench.Frontend.Web.Modules
{
    using Models;
    using Nancy;
    using Nancy.Security;

    public class SecureModule : NancyModule
    {
        public SecureModule() : base("/secure")
        {
            this.RequiresAuthentication();

            Get["/"] = x =>
            {
                var model = new UserModel(Context.CurrentUser.UserName);
                return View["Dashboard.cshtml", model];
            };
        }
    }
}