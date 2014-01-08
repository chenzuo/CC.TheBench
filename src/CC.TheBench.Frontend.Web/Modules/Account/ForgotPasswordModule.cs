namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Data;

    public class ForgotPasswordModule : BaseModule
    {
        public ForgotPasswordModule(IReadStoreFactory readStoreFactory)
            : base(readStoreFactory, "/account/forgotpassword")
        {
            Get["/"] = x =>
            {
                return View["account/forgotpassword"];
            };

            Post["/"] = _ => "register";
        }
    }
}