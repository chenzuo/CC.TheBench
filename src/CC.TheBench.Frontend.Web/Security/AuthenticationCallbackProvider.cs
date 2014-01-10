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

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            User loggedInUser = null;

            // If we are already logged in, get the current user
            if (nancyModule.Context.CurrentUser != null)
            {
                var email = ((UserIdentity)nancyModule.Context.CurrentUser).Email;
                loggedInUser = ReadStore.Users.Get(email).FirstOrDefault();
            }

            if (model.Exception == null)
            {
                var userInfo = model.AuthenticatedClient.UserInformation;
                var providerName = model.AuthenticatedClient.ProviderName;

                // See if we already know about this provider/id
                User user = ReadStore.Users.WithUserIdentities.FindAllByProviderNameAndId(providerName, userInfo.Id).FirstOrDefault();

                if (user == null)
                {
                    // User with that identity doesn't exist, check if a user is logged in
                    if (loggedInUser != null)
                    {
                        // Link to the logged in user
                        //LinkIdentity(userInfo, providerName, loggedInUser);

                        // If a user is already logged in, then we know they could only have gotten here via the account page,
                        // so we will redirect them there
                        //nancyModule.AddAlertMessage("success", string.Format("Successfully linked {0} account.", providerName));
                        return nancyModule.Response.AsRedirect("~/account/#identityProviders");
                    }
                    else
                    {
                        // User is unknown, nobody is logged in, send to register/link page

                    }
                }
                else if (loggedInUser != null && user != loggedInUser)
                {
                    // You can't link an account that's already attached to another user
                    //nancyModule.AddAlertMessage("error", string.Format("This {0} account has already been linked to another user.", providerName));

                    // If a user is logged in then we know they got here from the account page, and we should redirect them back there
                    return nancyModule.Response.AsRedirect("~/account/#identityProviders");
                }

                //return nancyModule.CompleteLogin(_authenticationTokenService, user);
                return null;
            }

            //nancyModule.AddAlertMessage("error", model.Exception.Message);

            // If a user is logged in, then they got here from the account page, send them back there
            if (loggedInUser != null)
            {
                return nancyModule.Response.AsRedirect("~/account/#identityProviders");
            }

            // At this point, send the user back to the root, everything else will work itself out
            return nancyModule.Response.AsRedirect("~/");

            // TODO: Look at how Jabbr does it: https://gist.github.com/CumpsD/8296129
            // need to check if ex is null, and then go on with ID + provider name
            //return nancyModule.View["AuthenticateCallback", model];
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