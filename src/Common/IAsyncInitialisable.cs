using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Provides a mechanism to initialise resources asynchronously.
    /// </summary>
    public interface IAsyncInitialisable
    {
        /// <summary>
        /// Performs application-defined tasks associated with initialising and 
        /// setting up resources asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous initialise operation.</returns>
        public ValueTask InitialiseAsync();
    }
}
