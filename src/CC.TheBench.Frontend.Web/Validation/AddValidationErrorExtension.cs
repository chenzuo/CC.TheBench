namespace CC.TheBench.Frontend.Web.Validation
{
    using Nancy;

    public static class AddValidationErrorExtension
    {
        public static void AddValidationError(this NancyModule module, string propertyName, string errorMessage)
        {
            module.ModelValidationResult.Errors.Add(propertyName, errorMessage);
        }

        public static void AddValidationError(this NancyModule module, string[] propertyNames, string errorMessage)
        {
            foreach (var propertyName in propertyNames)
                module.AddValidationError(propertyName, errorMessage);
        }
    }
}