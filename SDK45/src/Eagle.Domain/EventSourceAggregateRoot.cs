using Eagle.Core.Generators;
using Eagle.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eagle.Domain
{
    public class EventSourceAggregateRoot: IEventSourceAggregateRoot
    {
        private int id;
        private long version = 0;
        private long eventVersion = 0;
        private long branch = 0;

        private readonly List<IDomainEvent> uncommittedEvents = new List<IDomainEvent>();
        private readonly Dictionary<Type, List<object>> domainEventHandlers = new Dictionary<Type,List<object>>();

        public EventSourceAggregateRoot()
        {

        }

        #region Private methods

        private IEnumerable<object> GetDomainEventHandlers(IDomainEvent domainEvent)
        { 
            Type eventType = domainEvent.GetType();

            if (domainEventHandlers.ContainsKey(eventType))
            {
                return domainEventHandlers[eventType];
            }
            else
            {
                List<object> handlers = new List<object>();
                // firstly create and add all the handler methods defined within the aggregation root.
                MethodInfo[] allMethods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                var handlerMethods = from method in allMethods
                                     let returnType = method.ReturnType
                                     let @params = method.GetParameters()
                                     let handlerAttributes = method.GetCustomAttributes(typeof(DomainEventHandleAttribute), false)
                                     where returnType == typeof(void) &&
                                     @params != null &&
                                     @params.Count() > 0 &&
                                     @params[0].ParameterType.Equals(eventType) &&
                                     handlerAttributes != null &&
                                     ((DomainEventHandleAttribute)handlerAttributes[0]).DomainEventType.Equals(eventType)
                                     select new { MethodInfo = method };

                foreach (var handlerMethod in handlerMethods)
                {
                    var inlineDomainEventHandlerType = typeof(InlineDomainEventHandler<>).MakeGenericType(eventType);
                    var inlineDomainEventHandler = Activator.CreateInstance(inlineDomainEventHandlerType,
                        new object[] { this, handlerMethod.MethodInfo });

                    handlers.Add(inlineDomainEventHandler);
                }
                // then read all the registered handlers.
                domainEventHandlers.Add(eventType, handlers);
                return handlers;
            }
        }

        private void HandleEvent<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {

        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Raises a domain event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the domain event.</typeparam>
        /// <param name="event">The domain event to be raised.</param>
        protected virtual void RaiseEvent<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            @event.Id = (Guid)IdentityGenerator.Instance.Generate();
            @event.Version = ++eventVersion;
            @event.Source = this;
            @event.Branch = 0;
            @event.Timestamp = DateTime.UtcNow;

            this.HandleEvent<TEvent>(@event);
            this.uncommittedEvents.Add(@event);
        }

        #endregion

        public void LoadsFromHistory(IEnumerable<IDomainEvent> historicalEvents)
        {
            if (this.UncommittedChanges.Count() > 0)
            {
                this.UncommittedChanges.ToList().Clear();
            }

            foreach (IDomainEvent domainEvent in historicalEvents)
                this.HandleEvent<IDomainEvent>(domainEvent);

            this.version = historicalEvents.Last().Version;

            this.eventVersion = this.version;
        }

        public IEnumerable<IDomainEvent> UncommittedChanges
        {
            get 
            {
                return this.uncommittedEvents;
            }
        }

        public long Version
        {
            get 
            {
                return this.version;
            }
        }

        public long Branch
        {
            get 
            {
                return this.branch;
            }
        }

        public int Id
        {
            get 
            { 
                return this.id; 
            }
            set
            {
                this.id = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            EventSourceAggregateRoot other = obj as EventSourceAggregateRoot;
            if (obj == null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Utils.GetHashCode(this.Id.GetHashCode(),
                                     this.UncommittedChanges.GetHashCode(),
                                     this.Version.GetHashCode(),
                                     this.Branch.GetHashCode());
        }
    }
}
