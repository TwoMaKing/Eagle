using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Domain;
using Eagle.Domain.Events;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;
using Eagle.Tests.Domain.Services;

namespace Eagle.Tests.Domain.Events
{
    public class PostDomainEvent : DomainEvent
    {
        public PostDomainEvent(IEntity source) : base(source) { }

        public Post Post { get; set; }
    }
}
