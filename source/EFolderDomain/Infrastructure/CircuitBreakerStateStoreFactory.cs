using EFolderDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Infrastructure
{
    static class CircuitBreakerStateStoreFactory
    {
        public static ICircuitBreakerStateStore GetCircuitBreakerStateStore()
        {
            return new CircuitBreakerStateStore();
        }
    }
}
