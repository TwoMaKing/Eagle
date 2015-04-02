using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.IoC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Eagle.Core
{
    public sealed class ServiceLocator : IServiceLocator
    {
        private readonly static ServiceLocator instance = new ServiceLocator();

        private IObjectContainer objectContainer;

        private ServiceLocator() 
        {
            this.objectContainer = AppRuntime.Instance.CurrentApplication.ObjectContainer;
        }

        public static ServiceLocator Instance 
        {
            get 
            {
                return instance;
            }
        }

        public object GetService(Type serviceType)
        {
            return this.objectContainer.Resolve(serviceType);
        }

        public object GetService(Type serviceType, params ResolverParameter[] parameters)
        {
            return this.objectContainer.Resolve(serviceType, parameters);
        }

        public object GetService(Type serviceType, string name, params ResolverParameter[] parameters)
        {
            return this.objectContainer.Resolve(serviceType, name, parameters);
        }

        public TService GetService<TService>() 
        {
            return this.objectContainer.Resolve<TService>();
        }

        public TService GetService<TService>(params ResolverParameter[] parameters)
        {
            return this.objectContainer.Resolve<TService>(parameters);
        }

        public TService GetService<TService>(string name, params ResolverParameter[] overrides)
        {
            return this.objectContainer.Resolve<TService>(name, overrides);
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            return this.objectContainer.ResolveAll<TService>();
        }
    }
}
