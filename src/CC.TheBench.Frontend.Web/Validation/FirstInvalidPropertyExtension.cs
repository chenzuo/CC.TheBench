namespace CC.TheBench.Frontend.Web.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nancy.ViewEngines.Razor;

    public static class FirstInvalidPropertyExtension
    {
        public static string FirstInvalidProperty<T>(this HtmlHelpers<T> html, string defaultField, IEnumerable<string> fieldsOrder)
        {
            var validationResult = html.RenderContext.Context.ModelValidationResult;

            if (validationResult.IsValid)
                return defaultField;

            var properties = validationResult.ErrorProperties().ToList();

            foreach (var fieldToCheck in fieldsOrder)
            {
                if (properties.Contains(fieldToCheck, StringComparer.InvariantCultureIgnoreCase))
                    return fieldToCheck;
            }

            return defaultField;
        }
    }
}