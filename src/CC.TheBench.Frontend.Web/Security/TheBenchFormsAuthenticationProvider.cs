namespace CC.TheBench.Frontend.Web.Security
{
    using System.Threading.Tasks;
    using Microsoft.Owin.Security.Cookies;
    using Middleware;

    public class TheBenchFormsAuthenticationProvider : ICookieAuthenticationProvider
    {
        public Task ValidateIdentity(CookieValidateIdentityContext context)
        {
            return TaskHelper.Empty;
        }

        public void ResponseSignIn(CookieResponseSignInContext context)
        {
            
        }

        public void ApplyRedirect(CookieApplyRedirectContext context)
        {
            context.Response.Redirect(context.RedirectUri);
        }
    }
}