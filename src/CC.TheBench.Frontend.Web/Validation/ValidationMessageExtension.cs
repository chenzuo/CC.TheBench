namespace CC.TheBench.Frontend.Web.Validation
{
    using Nancy.ViewEngines.Razor;

    public static class ValidationMessageExtension
    {
        public static string ValidationMessage<T>(this HtmlHelpers<T> html, string validationFormat, string validationField)
        {
            const string fluentValidationMarker = "{PropertyName}";

            return validationFormat.Contains(fluentValidationMarker) 
                ? validationFormat.Replace(fluentValidationMarker, validationField) 
                : string.Format(validationFormat, validationField);
        }
    }
}