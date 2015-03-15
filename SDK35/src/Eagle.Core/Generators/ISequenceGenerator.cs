
namespace Eagle.Core.Generators
{
    /// <summary>
    /// Represents that the implemented classes are sequence generators.
    /// </summary>
    public interface ISequenceGenerator
    {
        /// <summary>
        /// Gets the next value of the sequence.
        /// </summary>
        object Next { get; }
    }
}