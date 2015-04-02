using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Eagle.Domain.Commands;

namespace Eagle.Domain.Bus
{
    public class CommandBus : ICommandBus
    {
        private Queue<ICommand> commandQueue = new Queue<ICommand>();

        private ICommand[] backupCommands;

        private bool committed;

        private ICommandDispatcher commandDispatcher;

        private MethodInfo dispatchMethod;

        public CommandBus(ICommandDispatcher commandDispatcher) 
        {
            this.commandDispatcher = commandDispatcher;

            this.dispatchMethod = this.commandDispatcher.GetType().GetMethod("Dispatch", BindingFlags.Public | BindingFlags.Instance);
        }

        public bool DistributedTransactionSupported
        {
            get 
            { 
                return false; 
            }
        }

        public bool Committed
        {
            get
            {
                return this.committed;
            }
        }

        public void Clear()
        {
            this.commandQueue.Clear();
        }

        public void Publish<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            this.commandQueue.Enqueue(command);
            this.committed = false;
        }

        public void Publish<TCommand>(IEnumerable<TCommand> commands) where TCommand : class, ICommand
        {
            if (commands != null)
            {
                foreach (TCommand command in commands) 
                {
                    this.Publish<TCommand>(command);
                }
            }
        }

        public void Commit()
        {
            this.backupCommands = new ICommand[this.commandQueue.Count];

            this.commandQueue.CopyTo(this.backupCommands, 0);

            while (this.commandQueue.Count > 0)
            {
                ICommand command = this.commandQueue.Dequeue();

                Type commandType = command.GetType();

                MethodInfo genericDispatchMethod = this.dispatchMethod.MakeGenericMethod(commandType);

                genericDispatchMethod.Invoke(this.commandDispatcher, new object[] { command });
            }

            this.committed = true;
        }

        public void Rollback()
        {
            if (this.backupCommands != null &&
                this.backupCommands.Length > 0)
            {
                this.Clear();

                foreach (ICommand command in this.backupCommands)
                {
                    this.commandQueue.Enqueue(command);
                }
            }

            this.committed = false;
        }

        public void Dispose()
        {
            this.Clear();
        }
    }
}
