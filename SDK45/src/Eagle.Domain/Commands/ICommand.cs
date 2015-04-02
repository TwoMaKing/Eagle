using Eagle.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Commands
{
    /// <summary>
    /// Used for CQRS. The implemented classes is commands  
    /// </summary>
    public interface ICommand : IEntity<Guid>
    {

    }
}
