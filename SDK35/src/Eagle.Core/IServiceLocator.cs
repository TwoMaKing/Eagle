using Eagle.Core.IoC;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Eagle.Core
{
    public interface IServiceLocator : IServiceProvider
    {
        object GetService(Type serviceType, params ResolverParameter[] parameters);

        object GetService(Type serviceType, string name, params ResolverParameter[] parameters);

        TService GetService<TService>(params ResolverParameter[] parameters);

        TService GetService<TService>(string name, params ResolverParameter[] parameters);

        IEnumerable<TService> ResolveAll<TService>();
    }
}
