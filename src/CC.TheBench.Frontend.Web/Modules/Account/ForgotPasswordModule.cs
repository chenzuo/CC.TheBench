namespace CC.TheBench.Frontend.Web.Modules.Account
{
    public class ForgotPasswordModule : BaseModule
    {
        public ForgotPasswordModule() : base("/account/forgotpassword")
        {
            Get["/"] = x =>
            {
                return View["account/forgotpassword"];
            };

            Post["/"] = _ => "register";
        }
    }
}