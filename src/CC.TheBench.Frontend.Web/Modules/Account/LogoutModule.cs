namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Nancy;
    using Nancy.Authentication.Forms;

    public class LogoutModule : NancyModule
    {
        public LogoutModule() : base("/account/logout")
        {
            Get["/"] = x => this.LogoutAndRedirect("~/");
        }
    }
}