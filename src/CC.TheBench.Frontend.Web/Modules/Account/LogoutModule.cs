namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Data;
    using Nancy.Authentication.Forms;

    public class LogoutModule : BaseModule
    {
        public LogoutModule(IReadStoreFactory readStoreFactory)
            : base(readStoreFactory, "/account/logout")
        {
            Get["/"] = x => this.LogoutAndRedirect("~/");
        }
    }
}