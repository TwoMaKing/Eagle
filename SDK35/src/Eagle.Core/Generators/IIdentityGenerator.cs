
namespace Eagle.Core.Generators
{
    /// <summary>
    /// Represents that the implemented classes are identity generators.
    /// </summary>
    public interface IIdentityGenerator
    {
        /// <summary>
        /// Generates the identity.
        /// </summary>
        /// <returns>The generated identity instance.</returns>
        object Generate();
    }
}
