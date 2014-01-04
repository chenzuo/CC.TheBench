namespace CC.TheBench.Frontend.Web.Modules
{
    using Nancy;

    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ => "Hi!";
        }
    }
}