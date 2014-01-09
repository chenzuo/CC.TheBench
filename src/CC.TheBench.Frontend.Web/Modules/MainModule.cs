namespace CC.TheBench.Frontend.Web.Modules
{
    using Data;

    public class MainModule : BaseModule
    {
        public MainModule(IReadStoreFactory readStoreFactory)
            : base(readStoreFactory)
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