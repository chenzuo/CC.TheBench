namespace CC.TheBench.Frontend.Web.Security
{
    using System.Collections.Generic;
    using Nancy;
    using Nancy.Owin;

    public static class IsLocalExtension
    {
        public static bool IsLocal(this NancyContext context)
        {
            var env = (IDictionary<string, object>)context.Items[NancyOwinHost.RequestEnvironmentKey];

            return env.ContainsKey("server.IsLocal") && (bool)env["server.IsLocal"];
        }
    }
}