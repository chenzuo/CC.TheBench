namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Nancy.Authentication.Forms;

    public class LogoutModule : BaseModule
    {
        public LogoutModule() : base("/account/logout")
        {
            Get["/"] = x => this.LogoutAndRedirect("~/");
        }
    }
}