using Eagle.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain
{
    /// <summary>
    /// Represents that the implemented classes are aggregate roots that
    /// support event sourcing mechanism.
    /// </summary>
    public interface IEventSourceAggregateRoot : IAggregateRoot
    {
        /// <summary>
        /// Builds the aggreate from the historial events.
        /// </summary>
        void LoadsFromHistory(IEnumerable<IDomainEvent> historicalEvents);
        
        /// <summary>
        /// Gets all the uncommitted events.
        /// </summary>
        IEnumerable<IDomainEvent> UncommittedChanges { get; }

        /// <summary>
        /// Gets the version of the aggregate.
        /// </summary>
        long Version { get; }
        /// <summary>
        /// Gets the branch on which the aggregate exists.
        /// </summary>
        long Branch { get; }
    }
}
