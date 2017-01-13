using EFolderDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EFolderDomain.Infrastructure
{
    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException(Exception ex) : base("CircuitBreakerOpenException", ex) { }
    }

    /*
     * The CircuitBreaker class prevents an application repeatedly trying to execute an operation that is likely to fail, allowing it to continue without waiting for the fault to be rectified or wasting CPU cycles 
     * while it determines that the fault is long lasting. This class unions two patterns together: Retry Pattern and Circuit Breaker Pattern.
     * It is singleton.
     */
    public class CircuitBreaker<T> where T:class
    {
        private readonly ICircuitBreakerStateStore stateStore = CircuitBreakerStateStoreFactory.GetCircuitBreakerStateStore();
        private readonly object halfOpenSyncObject = new object();
        private TimeSpan openToHalfOpenWaitTime = TimeSpan.FromSeconds(20); // Interval between retry call to remote service
        private int retryCount = 2;
        private int retryDelayPeriod = 1000; // millisecond

        public int RetryCount { get { return this.retryCount; } set { this.retryCount = value; } }
        public int RetryDelayPeriod { get { return this.retryDelayPeriod; } set { this.retryDelayPeriod = value; } }
        public int OpenToHalfOpenWaitTime { get { return this.openToHalfOpenWaitTime.Seconds; } set { this.openToHalfOpenWaitTime = TimeSpan.FromSeconds(value); } }

        public bool IsClosed { get { return stateStore.IsClosed; } }
        public bool IsOpen { get { return !IsClosed; } }

        public T ExecuteAction(Func<T> func)
        {
            if (IsOpen)
            {
                if (stateStore.LastStateChangedDateUtc + openToHalfOpenWaitTime <DateTime.UtcNow)
                {
                    bool lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(halfOpenSyncObject, ref lockTaken);
                        if (lockTaken)
                        {
                            // Set the circuit breaker state to HalfOpen.
                            stateStore.HalfOpen();

                            // Attempt the operation.
                            T result = func();
                            this.stateStore.Reset();
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        // If there is still an exception, trip the breaker again immediately.
                        this.stateStore.Trip(ex);
                        
                        // Throw the exception so that the caller knows which exception occurred.
                        throw;
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(halfOpenSyncObject);
                        }
                    }
                }
                // The Open timeout has not yet expired. Throw a CircuitBreakerOpen exception to
                // inform the caller that the caller that the call was not actually attempted, 
                // and return the most recent exception received.
                throw new CircuitBreakerOpenException(stateStore.LastException);
            }

            // The circuit breaker is Closed, execute the action.
            try
            {
                return this.OperationWithBasicRetry(func);
            }
            catch (Exception ex)
            {
                // If an exception still occurs here, simply 
                // re-trip the breaker immediately.
                this.TrackException(ex);
                throw;
            }
        }
        protected T OperationWithBasicRetry(Func<T> func)
        {
            int currentRetry = 0;
            T result = null;

            for (;;)
            {
                try
                {
                    // Calling external service.
                    result = func();
                    break;
                }
                catch (Exception ex)
                {
                    currentRetry++;

                    if (currentRetry > this.retryCount || !IsTransient(ex))
                    {
                        throw;
                    }
                }
                Task.Delay(this.retryDelayPeriod);
            }
            return result;
        }

        private bool IsTransient(Exception ex)
        {
            var webException = ex as WebException;
            if (webException != null)
            {
                // If the web exception contains one of the following status values 
                // it may be transient.
                return new[] {WebExceptionStatus.ConnectionClosed,
                  WebExceptionStatus.Timeout,
                  WebExceptionStatus.RequestCanceled }.
                        Contains(webException.Status);
            }

            return false;
        }

        private void TrackException(Exception ex)
        {
            this.stateStore.Trip(ex);
        }
    }
}
