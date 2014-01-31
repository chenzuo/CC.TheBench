namespace CC.TheBench.Frontend.Web.Validation
{
    using System.Collections.Generic;
    using Nancy.Validation;

    public static class ErrorPropertiesExtension
    {
        public static IEnumerable<string> ErrorProperties(this ModelValidationResult validationResult)
        {
            return validationResult.IsValid
                ? new List<string>()
                : validationResult.Errors.Keys;
        }
    }
}