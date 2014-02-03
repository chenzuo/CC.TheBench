namespace CC.TheBench.Frontend.Web.Storage.Utilities
{
    using System;
    using System.IO;
    using System.Net;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue.Protocol;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Shared.Protocol;
    using Microsoft.WindowsAzure.Storage.Table.Protocol;

    internal partial class RetryPolicies
    {
        /// <summary>Similar to <see cref="TransientServerErrorBackOff"/>, yet
        /// the Table Storage comes with its own set or exceptions/.</summary>
        public IRetryPolicy TransientTableErrorBackOff()
        {
            return new TransientTableErrorBackOffRetry();
        }

        internal class TransientTableErrorBackOffRetry : IRetryPolicy
        {
            public IRetryPolicy CreateInstance()
            {
                return new TransientTableErrorBackOffRetry();
            }

            public bool ShouldRetry(int currentRetryCount, int statusCode, Exception lastException,
                out TimeSpan retryInterval,
                OperationContext operationContext)
            {
                if (currentRetryCount >= 30 || !TransientTableErrorExceptionFilter(lastException))
                {
                    retryInterval = TimeSpan.Zero;
                    return false;
                }

                // quadratic backoff, capped at 5 minutes
                var c = currentRetryCount + 1;
                retryInterval = TimeSpan.FromSeconds(Math.Min(300, c * c));

                return true;
            }
        }

        static bool TransientTableErrorExceptionFilter(Exception exception)
        {
            if (exception is AggregateException)
            {
                exception = exception.GetBaseException();
            }

            var storageException = exception as StorageException;
            if (storageException != null && storageException.InnerException != null)
            {
                return TransientTableErrorExceptionFilter(storageException.InnerException);
            }

            //var dataServiceRequestException = exception as DataServiceRequestException;
            //if (dataServiceRequestException != null)
            //{
            //    if (IsErrorStringMatch(GetErrorCode(dataServiceRequestException),
            //        StorageErrorCodeStrings.InternalError,
            //        StorageErrorCodeStrings.ServerBusy,
            //        StorageErrorCodeStrings.OperationTimedOut,
            //        TableErrorCodeStrings.TableServerOutOfMemory))
            //    {
            //        return true;
            //    }
            //}

            //var dataServiceQueryException = exception as DataServiceQueryException;
            //if (dataServiceQueryException != null)
            //{
            //    if (IsErrorStringMatch(GetErrorCode(dataServiceQueryException),
            //        StorageErrorCodeStrings.InternalError,
            //        StorageErrorCodeStrings.ServerBusy,
            //        StorageErrorCodeStrings.OperationTimedOut,
            //        TableErrorCodeStrings.TableServerOutOfMemory))
            //    {
            //        return true;
            //    }
            //}

            // The remote server returned an error: (500) Internal Server Error, or some timeout
            // The server should not timeout in theory (that's why there are limits and pagination)
            var webException = exception as WebException;
            if (webException != null &&
                (webException.Status == WebExceptionStatus.ProtocolError ||
                 webException.Status == WebExceptionStatus.ConnectionClosed ||
                 webException.Status == WebExceptionStatus.ConnectFailure ||
                 webException.Status == WebExceptionStatus.Timeout))
            {
                return true;
            }

            var ioException = exception as IOException;
            if (ioException != null)
            {
                return true;
            }

            // HACK: StorageClient does not catch internal errors very well.
            // Hence we end up here manually catching exception that should have been correctly
            // typed by the StorageClient:

            // System.Net.InternalException is internal, but uncaught on some race conditions.
            // We therefore assume this is a transient error and retry.
            var exceptionType = exception.GetType();
            if (exceptionType.FullName == "System.Net.InternalException")
            {
                return true;
            }

            return false;
        }

    }
}