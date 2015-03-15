using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Eagle.Core.Application;
using Eagle.Core.Configuration;

namespace Eagle.Domain
{
    public class ConfigSourceHandlerProvider : IHandlerProvider
    {
        public IDictionary<Type, Type> GetHandlers()
        {
            return null;
        }
    }
}
