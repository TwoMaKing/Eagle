using Eagle.Core.Application;
using Eagle.Core.Exceptions;
using System;

namespace Eagle.Core.Generators
{
    /// <summary>
    /// Represents the default identity generator.
    /// </summary>
    public sealed class IdentityGenerator : IIdentityGenerator
    {
        #region Private Fields
        private static readonly IdentityGenerator instance = new IdentityGenerator();
        private readonly IIdentityGenerator generator = null;
        #endregion

        #region Ctor
        static IdentityGenerator() { }

        private IdentityGenerator()
        {
            if (AppRuntime.Instance.CurrentApplication == null)
            {
                throw new EagleAppException("The application has not been initialized and started yet.");
            }

            if (AppRuntime.Instance.CurrentApplication.ConfigSource == null ||
                AppRuntime.Instance.CurrentApplication.ConfigSource.Config == null ||
                AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators == null ||
                AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators.IdentityGenerator == null ||
                string.IsNullOrEmpty(AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators.IdentityGenerator.Provider))
            {
                generator = new SequentialIdentityGenerator();
            }
            else
            {
                Type type = Type.GetType(AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators.IdentityGenerator.Provider);

                if (type == null)
                {
                    throw new ConfigException("Unable to create the type from the name {0}.", AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators.IdentityGenerator.Provider);
                }

                if (type.Equals(this.GetType()))
                {
                    throw new EagleAppException("Type {0} cannot be used as identity generator, it is maintained by the Eagle framework internally.", this.GetType().AssemblyQualifiedName);
                }

                generator = (IIdentityGenerator)Activator.CreateInstance(type);
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the instance of the <c>IdentityGenerator</c> class.
        /// </summary>
        public static IdentityGenerator Instance
        {
            get { return instance; }
        }
        #endregion

        #region IIdentityGenerator Members
        /// <summary>
        /// Generates the identity.
        /// </summary>
        /// <returns>The generated identity instance.</returns>
        public object Generate()
        {
            return generator.Generate();
        }

        #endregion
    }
}
