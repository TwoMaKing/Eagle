﻿using Eagle.Common.Compression;
using Eagle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.MessageQueue
{
    public interface IMessageQueueBus<TMessage> : IUnitOfWork, IDisposable where TMessage : class
    {
        void Publish(TMessage message);

        void Publish(IEnumerable<TMessage> messages);
    }
}
