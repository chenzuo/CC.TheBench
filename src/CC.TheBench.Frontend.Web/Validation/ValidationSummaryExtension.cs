namespace CC.TheBench.Frontend.Web.Validation
{
    using System.Linq;
    using Nancy.ViewEngines.Razor;

    public static class ValidationSummaryExtension
    {
        public static IHtmlString ValidationSummary<T>(this HtmlHelpers<T> html)
        {
            const string errorDialog = @"<div id=""errors"" class=""alert alert-danger""{0}>{1}</div>";

            var isValid = html.RenderContext.Context.ModelValidationResult.IsValid;

            var errorDialogVisible = isValid
                ? "style=\"display: none;\""
                : string.Empty;

            var errors = isValid
                ? string.Empty
                : string.Concat(html.RenderContext.Context.ModelValidationResult.Errors.Values.SelectMany(x => x.Select(y => string.Concat("<p>", y.ErrorMessage, "</p>"))));

            return new NonEncodedHtmlString(string.Format(errorDialog, errorDialogVisible, errors));
        } 
    }
}