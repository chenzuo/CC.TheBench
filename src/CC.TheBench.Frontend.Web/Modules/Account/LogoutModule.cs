namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Nancy;
    using Security.Extensions;

    public class LogoutModule : BaseModule
    {
        public LogoutModule()
            : base("/account/logout")
        {
            Get["/"] = x =>
            {
                this.SignOut();

                return Response.AsRedirect("~/");
            };
        }
    }
}