namespace CC.TheBench.Frontend.Web.Validation
{
    using System;
    using System.Linq;
    using Nancy.ViewEngines.Razor;

    public static class ValidationStyleExtension
    {
        public static string ValidationStyle<T>(this HtmlHelpers<T> html, string property, string cssClass)
        {
            var validationResult = html.RenderContext.Context.ModelValidationResult;

            if (validationResult.IsValid)
                return string.Empty;

            return validationResult.ErrorProperties().Contains(property, StringComparer.InvariantCultureIgnoreCase)
                ? cssClass
                : string.Empty;
        }
    }
}