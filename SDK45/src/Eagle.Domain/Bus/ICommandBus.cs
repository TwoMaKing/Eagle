using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core;
using Eagle.Domain.Commands;

namespace Eagle.Domain.Bus
{
    public interface ICommandBus : IUnitOfWork, IDisposable
    {
        void Clear();

        void Publish<TCommand>(TCommand command) where TCommand : class, ICommand;

        void Publish<TCommand>(IEnumerable<TCommand> commands) where TCommand : class, ICommand;
    }
}
