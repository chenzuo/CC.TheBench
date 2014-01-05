namespace CC.TheBench.Frontend.Web.Views.Account.Models
{
    using FluentValidation;
    using Resources;

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithName(Account.Email).WithMessage(Validation.IsRequired);

            RuleFor(x => x.Password).NotEmpty().WithName(Account.Password).WithMessage(Validation.IsRequired);
        }
    }
}