namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using System;
    using System.Dynamic;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Extensions;
    using Security;

    public class LoginModule : NancyModule
    {
        public LoginModule() : base("/account/login")
        {
            Get["/"] = x =>
            {
                dynamic model = new ExpandoObject();
                model.Errored = Request.Query.error.HasValue;

                return View["index", model];
            };

            Post["/"] = x =>
            {
                var userGuid = UserDatabase.ValidateUser((string)Request.Form.Username, (string)Request.Form.Password);

                if (userGuid == null)
                {
                    return Context.GetRedirect("~/account/login?error=true&username=" + (string)Request.Form.Username);
                }

                DateTime? expiry = null;
                if (Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };
        }
    }
}