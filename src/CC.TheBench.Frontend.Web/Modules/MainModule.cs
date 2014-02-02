namespace CC.TheBench.Frontend.Web.Modules
{
    public class MainModule : BaseModule
    {
        public MainModule()
        {
            Get["/"] = _ =>
            {
                // TODO: Just for testing, remove later
                //ReadStore.Photos.FindAllByPublished(true);

                //Context.Culture = new CultureInfo("nl-BE");
                return View["index"];
            };
        }
    }
}