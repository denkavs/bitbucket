using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFolderDomain.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace UnitTestEFolderBackUp
{
    [TestClass]
    public class CircuitBreakerTest
    {
        private CircuitBreaker<string> breaker = new CircuitBreaker<string>();

        [TestMethod]
        public void SimpleCall_Test()
        {
            string res = breaker.ExecuteAction(() => {
                string result = "Successfull call";
                return result;
            });

            Assert.AreEqual("Successfull call", res);
        }

        [TestMethod]
        public void BasicRetryCall_Successfull()
        {
            breaker.RetryCount = 3;
            int realCount = 2;

            string res = breaker.ExecuteAction(() => {
                WebException ex = new WebException("Invalidate connection", WebExceptionStatus.ConnectionClosed);
                string result = "Successfull call";
                if (realCount != 0)
                {
                    realCount--;
                    throw ex;
                }

                return result;
            });

            Assert.AreEqual("Successfull call", res);
            Assert.AreEqual(0, realCount);
            Assert.IsTrue(breaker.IsClosed);
        }

        [TestMethod]
        public void BasicRetryCall_Failed_RetryCount_excised()
        {
            breaker.RetryCount = 2;
            int realCount = 4;
            string res = string.Empty;
            try
            {
                res = breaker.ExecuteAction(() => {
                    WebException ex = new WebException("Invalidate connection", WebExceptionStatus.ConnectionClosed);
                    if (realCount != 0)
                    {
                        realCount--;
                        throw ex;
                    }

                    string result = "Successfull call";
                    return result;
                });
            }
            catch(WebException ex)
            {
                Assert.AreEqual(WebExceptionStatus.ConnectionClosed, ex.Status);
            }
            Assert.AreEqual(1, realCount);
            Assert.IsTrue(breaker.IsOpen);
        }

        [TestMethod]
        public void BasicRetryCall_Failed_NotWebException_occured ()
        {
            breaker.RetryCount = 2;
            int realCount = 2;
            string res = string.Empty;
            try
            {
                res = breaker.ExecuteAction(() => {
                    WebException ex = new WebException("Invalidate connection", WebExceptionStatus.ConnectionClosed);
                    if (realCount != 1)
                    {
                        realCount--;
                        throw ex;
                    }
                    else
                    {
                        throw new OutOfMemoryException("Out of memory");
                    }

                    string result = "Successfull call";
                    return result;
                });
            }
            catch (OutOfMemoryException ex)
            {
            }
            Assert.AreEqual(1, realCount);
            Assert.IsTrue(breaker.IsOpen);
        }

        [TestMethod]
        public void BasicRetryCall_Failed_CircuitBreakerOpenException_occured()
        {
            breaker.RetryCount = 1;
            int realCount = 2;
            string res = string.Empty;
            try
            {
                res = breaker.ExecuteAction(() => {
                    WebException ex = new WebException("Invalidate connection", WebExceptionStatus.ConnectionClosed);
                    if (realCount != 0)
                    {
                        realCount--;
                        throw ex;
                    }
                    string result = "Successfull call";
                    return result;
                });
            }
            catch (WebException ex)
            {
            }
            Assert.AreEqual(0, realCount);
            Assert.IsTrue(breaker.IsOpen);
            bool isServiceCalled = false;
            try
            {
                res = breaker.ExecuteAction(() => { isServiceCalled = true; return string.Empty; });
            }
            catch(CircuitBreakerOpenException ex)
            {
                WebException we = ex.InnerException as WebException;
                if (we == null || we.Status != WebExceptionStatus.ConnectionClosed)
                    throw;
            }

            Assert.IsTrue(!isServiceCalled);
            Assert.IsTrue(breaker.IsOpen);
        }

        [TestMethod]
        public void BasicRetryCall_Failed_WaitingTimeExpired_SuccessfullCall()
        {
            breaker.OpenToHalfOpenWaitTime = 3;
            breaker.RetryCount = 1;
            int realCount = 2;
            string res = string.Empty;
            try
            {
                res = breaker.ExecuteAction(() => {
                    WebException ex = new WebException("Invalidate connection", WebExceptionStatus.ConnectionClosed);
                    if (realCount != 0)
                    {
                        realCount--;
                        throw ex;
                    }
                    string result = "Successfull call";
                    return result;
                });
            }
            catch (WebException ex)
            {
            }
            Assert.AreEqual(0, realCount);
            Assert.IsTrue(breaker.IsOpen);
            bool isServiceCalled = false;
            try
            {
                res = breaker.ExecuteAction(() => { isServiceCalled = true; return string.Empty; });
            }
            catch (CircuitBreakerOpenException ex)
            {
                WebException we = ex.InnerException as WebException;
                if (we == null || we.Status != WebExceptionStatus.ConnectionClosed)
                    throw;
            }
            Assert.IsTrue(!isServiceCalled);
            Assert.IsTrue(breaker.IsOpen);

            Thread.Sleep(5000);
            res = breaker.ExecuteAction(() => { isServiceCalled = true; return "Successfull call"; });
            Assert.AreEqual("Successfull call", res);
            Assert.AreEqual(true, isServiceCalled);
            Assert.IsTrue(breaker.IsClosed);

            ResetOpenToHalfOpenWaitTime();
        }

        [TestMethod]
        public void BasicRetryCall_Failed_WaitingTimeExpired_FailedCall()
        {
            breaker.OpenToHalfOpenWaitTime = 3;
            breaker.RetryCount = 1;
            int realCount = 2;
            string res = string.Empty;
            try
            {
                res = breaker.ExecuteAction(() => {
                    WebException ex = new WebException("Invalidate connection", WebExceptionStatus.ConnectionClosed);
                    if (realCount != 0)
                    {
                        realCount--;
                        throw ex;
                    }
                    string result = "Successfull call";
                    return result;
                });
            }
            catch (WebException ex)
            {
            }
            Assert.AreEqual(0, realCount);
            Assert.IsTrue(breaker.IsOpen);

            Thread.Sleep(5000);
            bool isServiceCalled = false;
            try
            {
                res = breaker.ExecuteAction(() => { isServiceCalled = true; throw new WebException("Failed call again. Status Opened", WebExceptionStatus.ConnectFailure); return string.Empty; });
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.ConnectFailure || ex.Status != WebExceptionStatus.ConnectFailure)
                    throw;
            }
            Assert.IsTrue(isServiceCalled);
            Assert.IsTrue(breaker.IsOpen);
            ResetOpenToHalfOpenWaitTime();
        }

        private void ResetOpenToHalfOpenWaitTime()
        {
            if(this.breaker != null)
            {
                this.breaker.OpenToHalfOpenWaitTime = 20;
            }
        }
    }
}
