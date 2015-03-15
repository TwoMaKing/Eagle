﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core;
using Eagle.Domain.Events;

namespace Eagle.Domain.Bus
{
    public interface IEventBus : IUnitOfWork, IDisposable
    {
        void Clear();

        void Publish<TEvent>(TEvent @event) where TEvent : class, IEvent;

        void Publish<TEvent>(IEnumerable<TEvent> events) where TEvent : class, IEvent;
    }
}
