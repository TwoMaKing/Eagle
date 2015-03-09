using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Transactions
{
    public sealed class TransactionCoordinatorFacotry
    {
        public static ITransactionCoordinator Create() 
        {
            return new DistributedTransactionCoordinator();    
        }
    }
}
