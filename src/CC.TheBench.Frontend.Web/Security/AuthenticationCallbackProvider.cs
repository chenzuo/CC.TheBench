namespace CC.TheBench.Frontend.Web.Security
{
    using Data;
    using Data.ReadModel;
    using Nancy;
    using Nancy.SimpleAuthentication;

    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        private dynamic ReadStore { get; set; }

        public AuthenticationCallbackProvider(IReadStoreFactory readStoreFactory)
        {
            ReadStore = readStoreFactory.ReadStore();
        }

        /// <summary>
        /// Called when we receive a callback from a social provider
        /// </summary>
        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            // Get the currently logged in user
            var loggedInUser = GetLoggedInUser(nancyModule);

            // TODO: Look at https://github.com/JabbR/JabbR/blob/cea55e08d9507b3570d4c1f0b2af6db2a9e44aaa/JabbR/Nancy/JabbRAuthenticationCallbackProvider.cs to get the returnurl stuff

            if (model.Exception == null)
            {
                var userInfo = model.AuthenticatedClient.UserInformation;
                var providerName = model.AuthenticatedClient.ProviderName;

                // See if we already know about this provider/id
                UserIdentity userIdentity = ReadStore.UserIdentities.FindAllByProviderAndId(providerName, userInfo.Id).FirstOrDefault();

                // Deal with an unknown/already known social identity
                return userIdentity == null 
                    ? HandleUnknownIdentity(loggedInUser, nancyModule) 
                    : HandleKnownIdentity(loggedInUser, userIdentity, nancyModule);
            }

            // An error occured, we didn't get permission, etc...
            //nancyModule.AddAlertMessage("error", model.Exception.Message);

            // If a user was logged in, he got here from the linking page
            // If the user isn't logged in, he got here from the login page
            return nancyModule.Response.AsRedirect(loggedInUser != null ? "~/account/#identityProviders" : "~/account/login");
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            return null;

            //when we redirect
            //we take the key/secret, pass info to google, get a token url
            //then redirect to the login page using that information
            //if that request is bung, we throw the OnRedirectToAuthenticationProviderError
            //with the info
            //once that stuff is working, it should never break
            //unless you change something, or the provider changes something
        }

        /// <summary>
        /// Get the currently logged in user
        /// </summary>
        private User GetLoggedInUser(INancyModule nancyModule)
        {
            var currentUser = nancyModule.Context.CurrentUser;
            if (currentUser == null) 
                return null;

            //var email = ((TheBenchIdentity)currentUser).Email;
            //User loggedInUser = ReadStore.Users.Get(email).FirstOrDefault();
            //return loggedInUser;
            return null;
        }

        /// <summary>
        /// Deal with an unknown social identity
        /// </summary>
        private dynamic HandleUnknownIdentity(User loggedInUser, INancyModule nancyModule)
        {
            // User with that identity doesn't exist, check if a user is logged in
            if (loggedInUser != null)
            {
                // Link to the logged in user
                // TODO: Implement
                //LinkIdentity(userInfo, providerName, loggedInUser);

                // If a user is already logged in, then we know they could only have gotten here via the account page,
                // so we will redirect them there
                //nancyModule.AddAlertMessage("success", string.Format("Successfully linked {0} account.", providerName));
                return nancyModule.Response.AsRedirect("~/account/identityProviders");
            }

            // User is unknown, nobody is logged in, send to register/link page
            // TODO: Prefill model with email if we have it, and a flag saying it comes from a social identity
            return nancyModule.Response.AsRedirect("~/account/register");
        }

        /// <summary>
        /// Deal with an already known social identity
        /// </summary>
        private dynamic HandleKnownIdentity(User loggedInUser, UserIdentity userIdentity, INancyModule nancyModule)
        {
            // If we aren't logged in, log ourselves in
            if (loggedInUser == null)
            {
                // TODO: Implement
                //return nancyModule.CompleteLogin(_authenticationTokenService, user);
                return null;
            }

            // If we are logged in, we are trying to link ourselves, check if we are allowed
            if (loggedInUser.UserId != userIdentity.UserId)
            {
                // You can't link an account that's already attached to another user
                // TODO: Implement
                //nancyModule.AddAlertMessage("error", string.Format("This {0} account has already been linked to another user.", providerName));
            }

            // We are logged in, and are trying to link ourselves to something that has already been linked, just redirect
            // TODO: Perhaps we should update the returned data at this time
            return nancyModule.Response.AsRedirect("~/account/identityProviders");
        }

        //private void LinkIdentity(UserInformation userInfo, string providerName, ChatUser user)
        //{
        //    // Link this new identity
        //    user.Identities.Add(new ChatUserIdentity
        //    {
        //        Email = userInfo.Email,
        //        Identity = userInfo.Id,
        //        ProviderName = providerName
        //    });

        //    _repository.CommitChanges();
        //}
    }
}