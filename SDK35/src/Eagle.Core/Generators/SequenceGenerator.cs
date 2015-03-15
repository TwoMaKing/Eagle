
using Eagle.Core.Application;
using Eagle.Core.Exceptions;
using System;
using System.Configuration;

namespace Eagle.Core.Generators
{
    /// <summary>
    /// Represents the default sequence generator.
    /// </summary>
    public sealed class SequenceGenerator : ISequenceGenerator
    {
        #region Private Fields
        private static readonly SequenceGenerator instance = new SequenceGenerator();
        private readonly ISequenceGenerator generator = null;
        #endregion

        #region Ctor
        static SequenceGenerator() { }

        private SequenceGenerator()
        {
            try
            {
                if (AppRuntime.Instance.CurrentApp == null)
                {
                    throw new EagleAppException("The application has not been initialized and started yet.");
                }

                if (AppRuntime.Instance.CurrentApp.ConfigSource == null ||
                    AppRuntime.Instance.CurrentApp.ConfigSource.Config == null ||
                    AppRuntime.Instance.CurrentApp.ConfigSource.Config.Generators == null ||
                    AppRuntime.Instance.CurrentApp.ConfigSource.Config.Generators.SequenceGenerator == null ||
                    string.IsNullOrEmpty(AppRuntime.Instance.CurrentApp.ConfigSource.Config.Generators.SequenceGenerator.Provider))
                {
                    generator = new SequentialIdentityGenerator();
                }
                else
                {
                    Type type = Type.GetType(AppRuntime.Instance.CurrentApp.ConfigSource.Config.Generators.SequenceGenerator.Provider);

                    if (type == null)
                    {
                        throw new ConfigException(string.Format("Unable to create the type from the name {0}.", AppRuntime.Instance.CurrentApp.ConfigSource.Config.Generators.SequenceGenerator.Provider));
                    }

                    if (type.Equals(this.GetType()))
                    {
                        throw new EagleAppException("Type {0} cannot be used as sequence generator, it is maintained by the Eagle framework internally.", this.GetType().AssemblyQualifiedName);
                    }

                    generator = (ISequenceGenerator)Activator.CreateInstance(type);
                }
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EagleAppException();
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the singleton instance of <c>SequenceGenerator</c> class.
        /// </summary>
        public static SequenceGenerator Instance
        {
            get { return instance; }
        }
        #endregion

        #region ISequenceGenerator Members
        /// <summary>
        /// Gets the next value of the sequence.
        /// </summary>
        public object Next
        {
            get { return generator.Next; }
        }

        #endregion
    }
}
