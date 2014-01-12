namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Data;
    using Nancy;
    using Security.Extensions;

    public class LogoutModule : BaseModule
    {
        public LogoutModule(IReadStoreFactory readStoreFactory)
            : base(readStoreFactory, "/account/logout")
        {
            Get["/"] = x =>
            {
                this.SignOut();

                return Response.AsRedirect("~/");
            };
        }
    }
}