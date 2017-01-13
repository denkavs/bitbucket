using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ConsoleBackUp.Infrastructure
{
    public class BackUpThrottleFilter : Attribute, IActionFilter
    {
        private static readonly ConcurrentThrottler throttler = new ConcurrentThrottler();
        private static int maxConcarrentOperation = GetConcarrentOperationFromConfig();

        static BackUpThrottleFilter() { }

        static int GetConcarrentOperationFromConfig()
        {
            return ConfMgr.GetMaxNumberOfBackUpConcarrentOperations();
        }

        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (throttler.Increment() <= BackUpThrottleFilter.maxConcarrentOperation)
            {
                HttpResponseMessage result = await continuation();
                throttler.Decrement();
                return result;
            }
            else
            {
                HttpResponseMessage response = actionContext.Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "Limit Reached");
                throttler.Decrement();
                return response;
            }
        }
    }
}
