using System;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Represents a service that can be used with dependency injection.
    /// </summary>
    public abstract class ServiceBase : IAsyncInitialisable, IDisposable, IAsyncDisposable
    {
        private bool initialised;
        private bool disposed;

        /// <inheritdoc/>
        public async ValueTask InitialiseAsync()
        {
            if (!initialised && !disposed)
            {
                initialised = true;
                await InitialiseServiceAsync();
            }
        }

        /// <summary>
        /// Initialises service-specific ressources asynchronously.
        /// </summary>
        /// <remarks>
        /// This method is called once during service initialisation with 
        /// <see cref="InitialiseAsync"/>.
        /// </remarks>
        /// <returns>A task that represents the asynchronous initialise operation.</returns>
        protected virtual ValueTask InitialiseServiceAsync()
            => new ValueTask();

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes service-specific unmanaged and, if called deterministically, managed ressources synchronously.
        /// </summary>
        /// <remarks>Subsequent calls to this method should do nothing.</remarks>
        /// <param name="disposing"><see langword="true"/> if called deterministically.</param>
#pragma warning disable IDE0060 // Remove unused parameter
        protected virtual void Dispose(bool disposing)
#pragma warning restore IDE0060 // Remove unused parameter
            => disposed = true;

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await DisposeServiceAsync();

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes service-specific managed ressources asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        protected virtual ValueTask DisposeServiceAsync()
            => default;

        /// <summary>
        /// Checks wether the service instance is in a valid state to 
        /// allow access to its members.
        /// </summary>
        /// <exception cref="InvalidOperationException">The service is not initialised.</exception>
        /// <exception cref="ObjectDisposedException">The service is already disposed.</exception>
        protected void CheckValidState()
        {
            if (!initialised)
            {
                throw new InvalidOperationException();
            }

            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}
