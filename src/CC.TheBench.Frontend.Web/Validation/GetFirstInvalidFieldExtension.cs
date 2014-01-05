namespace CC.TheBench.Frontend.Web.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nancy;

    public static class GetFirstInvalidFieldExtension
    {
        public static string GetFirstInvalidField(this NancyContext context, string defaultField, IEnumerable<string> fieldsOrder)
        {
            var validationResult = context.ModelValidationResult;

            if (validationResult.IsValid)
                return defaultField;

            var memberNames = validationResult.Errors.SelectMany(x => x.MemberNames).ToList();

            foreach (var fieldToCheck in fieldsOrder)
            {
                if (memberNames.Contains(fieldToCheck, StringComparer.InvariantCultureIgnoreCase))
                    return fieldToCheck;
            }

            return defaultField;
        }
    }
}