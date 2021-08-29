using System;

namespace ReactiveMarbles.MarbleLocator
{
    /// <summary>
    /// I Get Services.
    /// </summary>
    public interface IGetServices : IServiceProvider
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType">serviceType</paramref>.   -or-  null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.
        /// </returns>
        object GetService(Type serviceType, string? contract);

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>
        /// A service object[] of type <paramref name="serviceType">serviceType</paramref>.   -or-  null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.
        /// </returns>
        object[] GetServices(Type serviceType, string? contract = null);

        /// <summary>
        /// Determines whether the specified service type has service.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>
        ///   <c>true</c> if the specified service type has service; otherwise, <c>false</c>.
        /// </returns>
        bool HasService(Type serviceType, string? contract = null);
    }
}
