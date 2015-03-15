using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core;

namespace Eagle.Core.Transactions
{
    public interface ITransactionCoordinator : IUnitOfWork, IDisposable
    {

    }
}
