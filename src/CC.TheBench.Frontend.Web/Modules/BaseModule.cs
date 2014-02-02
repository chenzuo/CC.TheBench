namespace CC.TheBench.Frontend.Web.Modules
{
    using Nancy;
    using Security.Extensions;

    public abstract class BaseModule : NancyModule
    {
        protected bool IsAuthenticated
        {
            get { return this.IsAuthenticated(); }
        }

        protected BaseModule()
        {
        }

        protected BaseModule(string modulePath) : base(modulePath)
        {
        }
    }
}