namespace CC.TheBench.Frontend.Web.Modules
{
    using System;
    using System.Dynamic;
    using System.Globalization;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Extensions;
    using Security;

    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ =>
            {
                Context.Culture = new CultureInfo("nl-BE");
                return View["Index"];
            };

            Get["/login"] = x =>
            {
                dynamic model = new ExpandoObject();
                model.Errored = Request.Query.error.HasValue;

                return View["login", model];
            };

            Post["/login"] = x =>
            {
                var userGuid = UserDatabase.ValidateUser((string)Request.Form.Username, (string)Request.Form.Password);

                if (userGuid == null)
                {
                    return Context.GetRedirect("~/login?error=true&username=" + (string)Request.Form.Username);
                }

                DateTime? expiry = null;
                if (Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            Get["/logout"] = x => this.LogoutAndRedirect("~/");
        }
    }
}