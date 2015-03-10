﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Events
{
    /// <summary>
    /// Represent the class which implements the interface is a event data type.
    /// </summary>
    public interface IEvent : IEntity<Guid>
    {
        /// <summary>
        /// The date time when generates this event. It can be UTC time.
        /// </summary>
        DateTime Timestamp { get; set; }
    }
}
