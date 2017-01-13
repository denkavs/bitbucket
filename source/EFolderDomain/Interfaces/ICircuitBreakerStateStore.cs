using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Interfaces
{
    enum CircuitBreakerStateEnum
    {
        Open, // Prevents request to remote service
        HalfOpen, // One request to remote service is available. If it failed state is switched back to Open state.
        Closed // Remote service is available. Requests are allowed 
    }

    /*
     * Represents state information about a circuit breaker.
     */
    interface ICircuitBreakerStateStore
    {
        CircuitBreakerStateEnum State { get; }

        Exception LastException { get; }

        DateTime LastStateChangedDateUtc { get; }

        // The Trip method switches the state of the circuit breaker to the open state 
        void Trip(Exception ex);

        //The Reset method closes the circuit breaker
        void Reset();

        //HalfOpen method sets the circuit breaker to half-open
        void HalfOpen();

        bool IsClosed { get; }
    }
}
