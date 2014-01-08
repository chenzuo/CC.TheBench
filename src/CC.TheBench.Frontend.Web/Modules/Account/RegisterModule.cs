namespace CC.TheBench.Frontend.Web.Modules.Account
{
    public class RegisterModule : BaseModule
    {
        public RegisterModule() : base("/account/register")
        {
            Get["/"] = x =>
            {
                return View["account/register"];
            };

            Post["/"] = _ => "register";
        }
    }
}