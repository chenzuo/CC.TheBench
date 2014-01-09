namespace CC.TheBench.Frontend.Web.Utilities
{
    using System.Diagnostics;

    public interface IDebuggingService
    {
        bool RunningInDebugMode();
    }

    public class DebuggingService : IDebuggingService
    {
        private bool _debugging;

        public bool RunningInDebugMode()
        {
            //#if DEBUG
            //return true;
            //#else
            //return false;
            //#endif
            WellAreWe();
            return _debugging;
        }

        [Conditional("DEBUG")]
        private void WellAreWe()
        {
            _debugging = true;
        }
    }
}