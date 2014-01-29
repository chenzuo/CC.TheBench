namespace CC.TheBench.Frontend.Web.Utilities.Extensions.NancyExtensions
{
    using System.Collections.Generic;
    using Nancy;
    using Nancy.Validation;

    public static class AddValidationErrorExtension
    {
        public static void AddValidationError(this NancyModule module, string propertyName, string errorMessage)
        {
            module.ModelValidationResult.Errors.Add(propertyName, errorMessage);
        }

        public static void AddValidationError(this NancyModule module, string[] propertyNames, string errorMessage)
        {
            module.ModelValidationResult.Errors.Add(string.Join("-", propertyNames), propertyNames, errorMessage);
        }
    }

    public static class ModelValidationResultExtensions
    {
        /// <summary>
        /// Adds a new <see cref="ModelValidationError"/> to the validation results.
        /// </summary>
        /// <param name="errors">A reference to the <see cref="ModelValidationResult.Errors"/> property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="errorMessage">The validation error message.</param>
        /// <returns>A reference to the <see cref="ModelValidationResult.Errors"/> property.</returns>
        public static IDictionary<string, IList<ModelValidationError>> Add(this IDictionary<string, IList<ModelValidationError>> errors, string name, IEnumerable<string> memberNames, string errorMessage)
        {
            if (!errors.ContainsKey(name))
            {
                errors[name] = new List<ModelValidationError>();
            }

            errors[name].Add(new ModelValidationError(memberNames, errorMessage));

            return errors;
        }
    }
}