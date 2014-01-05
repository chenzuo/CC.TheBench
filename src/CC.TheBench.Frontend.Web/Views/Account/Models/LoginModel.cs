namespace CC.TheBench.Frontend.Web.Views.Account.Models
{
    using FluentValidation;
    using Resources;

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithName(Account.Username).WithMessage(Validation.IsRequired);

            RuleFor(x => x.Password).NotEmpty().WithName(Account.Password).WithMessage(Validation.IsRequired);
        }
    }
}