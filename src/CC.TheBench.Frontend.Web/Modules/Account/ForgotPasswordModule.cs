namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Nancy;

    public class ForgotPasswordModule : NancyModule
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