namespace CC.TheBench.Frontend.Web.Storage.Utilities
{
    using System;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue.Protocol;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Shared.Protocol;
    using Microsoft.WindowsAzure.Storage.Table.Protocol;

    internal partial class RetryPolicies
    {
        /// <summary>
        /// Very patient retry policy to deal with container, queue or table instantiation
        /// that happens just after a deletion.
        /// </summary>
        public IRetryPolicy SlowInstantiation()
        {
            return new SlowInstantiationRetry();
        }

        internal class SlowInstantiationRetry : IRetryPolicy
        {
            public IRetryPolicy CreateInstance()
            {
                return new SlowInstantiationRetry();
            }

            public bool ShouldRetry(int currentRetryCount, int statusCode, Exception lastException, out TimeSpan retryInterval, OperationContext operationContext)
            {
                if (currentRetryCount >= 30 || !SlowInstantiationExceptionFilter(lastException))
                {
                    retryInterval = TimeSpan.Zero;
                    return false;
                }

                // linear backoff
                retryInterval = TimeSpan.FromMilliseconds(100 * currentRetryCount);

                return true;
            }
        }

        private static bool SlowInstantiationExceptionFilter(Exception exception)
        {
            if (exception is AggregateException)
            {
                exception = exception.GetBaseException();
            }

            // Blob Storage or Queue Storage exceptions
            // Table Storage may throw exception of type 'StorageClientException'
            var storageException = exception as StorageException;
            if (storageException != null)
            {
                // 'client' exceptions reflect server-side problems (delayed instantiation)

                if (IsErrorStringMatch(storageException,
                    StorageErrorCodeStrings.ResourceNotFound,
                    StorageErrorCodeStrings.ContainerNotFound))
                {
                    return true;
                }

                if (IsErrorStringMatch(storageException,
                    QueueErrorCodeStrings.QueueNotFound,
                    QueueErrorCodeStrings.QueueBeingDeleted,
                    StorageErrorCodeStrings.InternalError,
                    StorageErrorCodeStrings.ServerBusy,
                    TableErrorCodeStrings.TableServerOutOfMemory,
                    TableErrorCodeStrings.TableNotFound,
                    TableErrorCodeStrings.TableBeingDeleted))
                {
                    return true;
                }

                if (storageException.InnerException != null)
                {
                    return SlowInstantiationExceptionFilter(storageException.InnerException);
                }
            }

            // Table Storage may also throw exception of type 'DataServiceQueryException'.
            //var dataServiceException = exception as DataServiceQueryException;
            //if (null != dataServiceException)
            //{
            //    if (IsErrorStringMatch(GetErrorCode(dataServiceException),
            //        TableErrorCodeStrings.TableBeingDeleted,
            //        TableErrorCodeStrings.TableNotFound,
            //        TableErrorCodeStrings.TableServerOutOfMemory))
            //    {
            //        return true;
            //    }
            //}

            return false;
        }
    }
}