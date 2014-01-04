namespace CC.TheBench.Frontend.Web.Modules
{
    using Nancy;

    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ =>
            {
                //Context.Culture = new CultureInfo("nl-BE");
                return View["index"];
            };
        }
    }
}