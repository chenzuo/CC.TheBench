namespace CC.TheBench.Frontend.Web.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class RequireHttpsHandler
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public RequireHttpsHandler(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public Task Invoke(IDictionary<string, object> env)
        {
            var request = new OwinRequest(env);
            var response = new OwinResponse(env);

            if (request.Uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase)) 
                return _next(env);

            var builder = new UriBuilder(request.Uri) { Scheme = "https" };

            if (request.Uri.IsDefaultPort)
                builder.Port = -1;

            response.Headers.Set("Location", builder.ToString());
            response.StatusCode = (int)HttpStatusCode.Found;

            return TaskHelper.Empty;
        }
    }
}