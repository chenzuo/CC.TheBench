namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using System;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Resources;
    using Security;
    using Views.Account.Models;

    public class LoginModule : NancyModule
    {
        public LoginModule() : base("/account/login")
        {
            Get["/"] = x =>
            {
                this.CreateNewCsrfToken();

                var model = this.Bind<LoginModel>();
                return View["account/login", model];
            };

            Post["/"] = x =>
            {
                this.ValidateCsrfToken();

                var model = this.BindAndValidate<LoginModel>();
                if (!ModelValidationResult.IsValid)
                    return View["account/login", model];

                var userGuid = UserDatabase.ValidateUser(model.Email, model.Password);

                if (userGuid == null)
                {
                    ModelValidationResult = ModelValidationResult.AddError(new[] { "Email", "Password" }, Account.InvalidLogin);
                    return View["account/login", model];
                }

                var expiry = (model.RememberMe) ? DateTime.Now.AddDays(7) : (DateTime?) null;

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };
        }
    }
}