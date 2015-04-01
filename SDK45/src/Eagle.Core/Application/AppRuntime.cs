using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Eagle.Core;
using Eagle.Core.Configuration;
using Eagle.Core.Exceptions;

namespace Eagle.Core.Application
{
    /// <summary>
    /// EApp Run time. Get the Global application objects/information.
    /// e.g. configuration, currently used IOC container, currently used plugin framework instaince.
    /// </summary>
    public sealed class AppRuntime
    {
        private static readonly AppRuntime appRuntime = new AppRuntime();

        private IApp currrentApplication;

        private AppRuntime() { }

        /// <summary>
        /// Singleton Pattern. Get the current instance of EAppRuntime
        /// </summary>
        public static AppRuntime Instance 
        {
            get 
            {
                return appRuntime;
            }
        }

        /// <summary>
        /// The instance of the current application.
        /// </summary>
        public IApp CurrentApplication 
        {
            get 
            {
                return this.currrentApplication;
            }
        }

        /// <summary>
        /// Initialize the instance of App by configuration in config file 
        /// </summary>
        public IApp Create(IConfigSource configSource) 
        {
            if (configSource.Config == null ||
                configSource.Config.Application == null)
            {
                throw new ConfigException("Either EApp configuration or EApp application configuration has not been initialized in the ConfigSource instance.");
            }

            string typeName = configSource.Config.Application.Provider;

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ConfigException("The provider type of the Application has not been defined in the ConfigSource yet.");
            }

            Type appType = Type.GetType(typeName);

            if (appType == null)
            {
                throw new InfrastructureException("The application provider defined by type '{0}' doesn't exist.", typeName);
            }

            IApp app = (IApp)Activator.CreateInstance(appType, new object[] { configSource });
            
            this.currrentApplication = app;

            return this.currrentApplication;
        }

        /// <summary>
        /// Initialize the instance of App by configuration in config file 
        /// </summary>
        public TApp Create<TApp>(IConfigSource configSource) where TApp : IApp
        {
            IApp app = this.Create(configSource);

            if (app != null)
            {
                return (TApp)app;
            }

            return default(TApp);
        }

        /// <summary>
        /// Initialize the default instance of App by directly reading config file.
        /// </summary>
        public IApp Create() 
        {
            return this.Create(new EAppConfigSource());
        }

        /// <summary>
        /// Get the specified App instance.
        /// </summary>
        public TApp GetApp<TApp>() where TApp : IApp
        {
            if (this.CurrentApplication != null)
            {
                return (TApp)this.CurrentApplication;
            }

            return default(TApp);
        }
    }
}
