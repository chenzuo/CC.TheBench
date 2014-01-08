namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Data;

    public class RegisterModule : BaseModule
    {
        public RegisterModule(IReadStoreFactory readStoreFactory) 
            : base(readStoreFactory, "/account/register")
        {
            Get["/"] = x =>
            {
                return View["account/register"];
            };

            Post["/"] = _ => "register";
        }
    }
}