namespace CC.TheBench.Frontend.Web.Storage.Utilities
{
    using System.Linq;
    using Microsoft.WindowsAzure.Storage;

    /// <summary>
    /// Azure retry policies for corner-situation and server errors.
    /// </summary>
    internal partial class RetryPolicies
    {
        private static bool IsErrorStringMatch(StorageException exception, params string[] errorStrings)
        {
            return exception != null && exception.RequestInformation.ExtendedErrorInformation != null && errorStrings.Contains(exception.RequestInformation.ExtendedErrorInformation.ErrorCode);
        }

        private static bool IsErrorStringMatch(string exceptionErrorString, params string[] errorStrings)
        {
            return errorStrings.Contains(exceptionErrorString);
        }
    }
}