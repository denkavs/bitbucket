using EFolderDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Infrastructure
{
    class CircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        private Exception lastException;
        private DateTime stateChangedDateUtc;
        private CircuitBreakerStateEnum state = CircuitBreakerStateEnum.Closed;

        public bool IsClosed
        {
            get
            {
                return this.state == CircuitBreakerStateEnum.Closed;
            }
        }

        public Exception LastException
        {
            get
            {
                return this.lastException;
            }
        }

        public DateTime LastStateChangedDateUtc
        {
            get
            {
                return this.stateChangedDateUtc;
            }
        }

        public CircuitBreakerStateEnum State
        {
            get
            {
                return this.state;
            }
        }

        public void HalfOpen()
        {
            this.state = CircuitBreakerStateEnum.HalfOpen;
        }

        public void Reset()
        {
            this.state = CircuitBreakerStateEnum.Closed;
        }

        public void Trip(Exception ex)
        {
            this.lastException = ex;
            this.state = CircuitBreakerStateEnum.Open;
            this.stateChangedDateUtc = DateTime.UtcNow;
        }
    }
}
