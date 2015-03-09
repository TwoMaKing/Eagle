using Eagle.Core;
using Eagle.Core.Application;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
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

        private IUnityContainer unityContainer = new UnityContainer();

        private ServiceLocator() 
        {
            if (AppRuntime.Instance.CurrentApp != null &&
                AppRuntime.Instance.CurrentApp.ObjectContainer != null)
            {
                this.unityContainer = AppRuntime.Instance.CurrentApp.ObjectContainer.GetWrapperContainer<IUnityContainer>();
            }
            else
            {
                UnityConfigurationSection unitySection = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

                unitySection.Configure(this.unityContainer);
            }
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
            return this.unityContainer.Resolve(serviceType);
        }

        public object GetService(Type serviceType, ResolverOverride[] overrides)
        {
            return this.unityContainer.Resolve(serviceType, overrides);
        }

        public object GetService(Type serviceType, string name, ResolverOverride[] overrides)
        {
            return this.unityContainer.Resolve(serviceType, name, overrides);
        }

        public TService GetService<TService>() 
        {
            return this.unityContainer.Resolve<TService>();
        }

        public TService GetService<TService>(ResolverOverride[] overrides)
        {
            return this.unityContainer.Resolve<TService>(overrides);
        }

        public TService GetService<TService>(string name)
        {
            return this.unityContainer.Resolve<TService>(name);
        }

        public TService GetService<TService>(string name, ResolverOverride[] overrides)
        {
            return this.unityContainer.Resolve<TService>(name, overrides);
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            return this.unityContainer.ResolveAll<TService>();
        }

    }
}
