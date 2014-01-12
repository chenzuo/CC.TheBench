namespace CC.TheBench.Frontend.Web.Middleware
{
    using System.Threading.Tasks;

    public class TaskHelper
    {
        private static readonly Task EmptyTask = MakeEmpty();

        private static Task MakeEmpty()
        {
            return FromResult<object>(null);
        }

        public static Task Empty
        {
            get { return EmptyTask; }
        }

        private static Task<T> FromResult<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }
    }
}