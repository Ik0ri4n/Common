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

        protected virtual ValueTask InitialiseServiceAsync()
            => new ValueTask();

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        protected virtual ValueTask DisposeServiceAsync()
            => default;

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
