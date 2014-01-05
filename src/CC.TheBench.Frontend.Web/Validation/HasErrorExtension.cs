namespace CC.TheBench.Frontend.Web.Validation
{
    using System.Linq;
    using Nancy;

    public static class HasErrorExtension
    {
        public static bool HasError(this NancyContext context, string memberName)
        {
            var validationResult = context.ModelValidationResult;

            if (validationResult.IsValid)
                return false;

            var memberNames = validationResult.Errors.SelectMany(x => x.MemberNames);

            return memberNames.Contains(memberName);
        }
    }
}