namespace CC.TheBench.Frontend.Web.Security
{
    using System;
    using Data;
    using Data.ReadModel;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;

    public class UserMapper : IUserMapper
    {
        private dynamic ReadStore { get; set; }

        public UserMapper(IReadStoreFactory readStoreFactory)
        {
            ReadStore = readStoreFactory.ReadStore();
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            User user = ReadStore.Users.FindAllByUserId(identifier).FirstOrDefault();

            return user == null
                ? null
                : new UserIdentity
                {
                    UserName = user.DisplayName,
                    Email = user.Email
                };

        }
    }
}