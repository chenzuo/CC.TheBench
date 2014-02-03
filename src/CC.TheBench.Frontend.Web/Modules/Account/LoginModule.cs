namespace CC.TheBench.Frontend.Web.Modules.Account
{
    using Data.ReadModel;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Resources;
    using Security;
    using Security.Extensions;
    using Storage;
    using Utilities.Extensions.NancyExtensions;
    using Validation;
    using Views.Account.Models;

    public class LoginModule : BaseModule
    {
        private readonly ISaltedHash _saltedHash;
        private readonly CloudTable<User> _users;

        public LoginModule(ISaltedHash saltedHash, CloudTable<User> users)
            : base("/account/login")
        {
            _saltedHash = saltedHash;
            _users = users;

            Get["/"] = x =>
            {
                if (IsAuthenticated)
                    return this.AsRedirectQueryStringOrDefault("~/dashboard");

                this.CreateNewCsrfToken();

                var model = this.Bind<LoginModel>();
                return View["account/login", model];
            };

            Post["/"] = x =>
            {
                if (IsAuthenticated)
                    return this.AsRedirectQueryStringOrDefault("~/dashboard");

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
            // TODO: Lets abstract this away later so we dont need to think about partitionkey and rowkey here
            var user = _users.Get(model.Email, "Manual");
            if (user == null)
                return null;

            return _saltedHash.VerifyHashString(model.Password, user.Value.HashAndSalt) 
                ? user.Value 
                : null;
        }

        private dynamic InvalidLogin(LoginModel model)
        {
            this.AddValidationError(new[] { "Email", "Password" }, Account.InvalidLogin);

            return View["account/login", model];
        }
    }
}