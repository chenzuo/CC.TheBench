namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Data;
    using Data.ReadModel;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Resources;
    using Security;
    using Security.Extensions;
    using Utilities.Extensions.NancyExtensions;
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
                if (IsAuthenticated)
                    return this.AsRedirectQueryStringOrDefault("~/");

                this.CreateNewCsrfToken();

                var model = this.Bind<LoginModel>();
                return View["account/login", model];
            };

            Post["/"] = x =>
            {
                if (IsAuthenticated)
                    return this.AsRedirectQueryStringOrDefault("~/");

                this.ValidateCsrfToken();

                var model = this.BindAndValidate<LoginModel>();
                if (!ModelValidationResult.IsValid)
                    return View["account/login", model];

                var user = VerifyUser(model);
                if (user == null)
                    return InvalidLogin(model);

                // TODO: Publish event for CQRS/ES audit logging
                return this.SignIn(user);
            };
        }

        private User VerifyUser(LoginModel model)
        {
            User user = ReadStore.Users.Get(model.Email);
            if (user == null)
                return null;

            return _saltedHash.VerifyHashString(model.Password, user.HashAndSalt) 
                ? user 
                : null;
        }

        private dynamic InvalidLogin(LoginModel model)
        {
            this.AddValidationError(new[] { "Email", "Password" }, Account.InvalidLogin);

            return View["account/login", model];
        }
    }
}