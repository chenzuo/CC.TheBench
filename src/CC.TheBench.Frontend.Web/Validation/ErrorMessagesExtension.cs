namespace CC.TheBench.Frontend.Web.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using Nancy.Validation;

    public static class ErrorMessagesExtension
    {
        public static IEnumerable<string> ErrorMessages(this ModelValidationResult validationResult)
        {
            return validationResult.IsValid
                ? new List<string>()
                : validationResult.Errors.Values.SelectMany(x => x.Select(y => y.ErrorMessage)).Distinct();
        }
    }
}