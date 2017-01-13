using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleBackUp.Infrastructure
{
    class ConcurrentThrottler
    {
        long _counter;

        public long Increment()
        {
            return Interlocked.Increment(ref _counter);
        }

        public long Decrement()
        {
            return Interlocked.Decrement(ref _counter);
        }

        internal static ConcurrentThrottler Zero
        {
            get
            {
                return new ConcurrentThrottler();
            }
        }
    }
}
