namespace CC.TheBench.Frontend.Web.Security
{
    using Nancy;
    using Nancy.SimpleAuthentication;

    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            // TODO: Look at how Jabbr does it: https://gist.github.com/CumpsD/8296129
            // need to check if ex is null, and then go on with ID + provider name
            return nancyModule.View["AuthenticateCallback", model];
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            throw new System.NotImplementedException();

            //when we redirect
            //we take the key/secret, pass info to google, get a token url
            //then redirect to the login page using that information
            //if that request is bung, we throw the OnRedirectToAuthenticationProviderError
            //with the info
            //once that stuff is working, it should never break
            //unless you change something, or the provider changes something
        }
    }
}