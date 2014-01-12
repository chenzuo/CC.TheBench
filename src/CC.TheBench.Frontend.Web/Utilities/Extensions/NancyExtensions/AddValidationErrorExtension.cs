namespace CC.TheBench.Frontend.Web.Utilities.Extensions.NancyExtensions
{
    using Nancy;

    public static class AddValidationErrorExtension
    {
        public static void AddValidationError(this NancyModule module, string propertyName, string errorMessage)
        {
            module.ModelValidationResult = module.ModelValidationResult.AddError(propertyName, errorMessage);
        }

        public static void AddValidationError(this NancyModule module, string[] propertyNames, string errorMessage)
        {
            module.ModelValidationResult = module.ModelValidationResult.AddError(propertyNames, errorMessage);
        }
    }
}