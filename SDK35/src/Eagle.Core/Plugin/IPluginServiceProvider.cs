using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Plugin
{
    public interface IPluginServiceProvider : IServiceProvider
    {
        object this[Type serviceType] { get; set; }

        bool Contains(Type serviceType);

        void AddService(Type serviceType, object value);

        void AddServices(IDictionary<Type, object> services);

        void RemoveService(Type serviceType);
    }
}
