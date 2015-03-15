﻿using Eagle.Domain;
using Eagle.Domain.Events;
using System;
using System.Linq;
using System.Reflection;

namespace Eagle.Domain.Events
{
    /// <summary>
    /// Represents the domain event handler that is defined within the scope of
    /// an aggregate root and handles the domain event when EventSourceAggregateRoot.RaiseEvent&lt;TEvent&gt;</c>
    /// is called.
    /// </summary>
    public sealed class InlineDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {
        #region Private Fields
        private readonly Type domainEventType;
        private readonly Action<TDomainEvent> action;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>InlineDomainEventHandler</c> class.
        /// </summary>
        /// <param name="aggregateRoot">The instance of the aggregate root within which the domain event
        /// was raised and handled.</param>
        /// <param name="mi">The method which handles the domain event.</param>
        public InlineDomainEventHandler(IEventSourceAggregateRoot aggregateRoot, MethodInfo mi)
        {
            ParameterInfo[] parameters = mi.GetParameters();
            if (parameters == null || parameters.Count() == 0)
            {
                throw new ArgumentException("The parameter of the method cannot be null or empty.");
            }

            domainEventType = parameters[0].ParameterType;

            this.action = domainEvent =>
            {
                try
                {
                    mi.Invoke(aggregateRoot, new object[] { domainEvent });
                }
                catch { }
            };
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Determines whether the specified System.Object is equal to the current System.Object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified System.Object is equal to the current System.Object;
        /// otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == (object)null)
                return false;
            InlineDomainEventHandler<TDomainEvent> other = obj as InlineDomainEventHandler<TDomainEvent>;
            if ((object)other == (object)null)
                return false;
            return Delegate.Equals(this.action, other.action);
        }
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current System.Object.</returns>
        public override int GetHashCode()
        {
            if (this.action != null && this.domainEventType != null)
            {
                return Utils.GetHashCode(this.action.GetHashCode(),
                                         this.domainEventType.GetHashCode());
            }

            return base.GetHashCode();
        }
        #endregion

        #region IHandler<TDomainEvent> Members
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle(TDomainEvent message)
        {
            this.action(message);
        }

        #endregion
    }
}
