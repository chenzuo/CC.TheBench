namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using System;
    using Data;
    using Data.ReadModel;
    using Nancy.Authentication.Forms;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Resources;
    using Security;
    using Views.Account.Models;

    public class LoginModule : BaseModule
    {
        private readonly ISaltedHash _saltedHash;

        public LoginModule(IReadStoreFactory readStoreFactory, ISaltedHash saltedHash)
            : base(readStoreFactory, "/account/login")
        {
            this.RequiresProductionHttps(true);

            _saltedHash = saltedHash;
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

                var user = VerifyUser(model);
                if (user == null)
                    return InvalidLogin(model);

                var expiry = model.RememberMe
                    ? DateTime.Now.AddDays(7)
                    : (DateTime?) null;

                // TODO: Publish event for CQRS/ES audit logging
                return this.LoginAndRedirect(user.UserId, expiry);
            };
        }

        private User VerifyUser(LoginModel model)
        {
            User user = ReadStore.Users.Get(model.Email);
            if (user == null)
                return null;

            return _saltedHash.VerifyHashString(model.Password, user.Hash, user.Salt) 
                ? user 
                : null;
        }

        private dynamic InvalidLogin(LoginModel model)
        {
            ModelValidationResult = ModelValidationResult.AddError(new[] { "Email", "Password" }, Account.InvalidLogin);
            return View["account/login", model];
        }
    }
}