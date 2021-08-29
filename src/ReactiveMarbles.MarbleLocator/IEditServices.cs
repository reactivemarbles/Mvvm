﻿using System;

namespace ReactiveMarbles.MarbleLocator
{
    /// <summary>
    /// I Edit Services.
    /// </summary>
    public interface IEditServices
    {
        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        void AddService(Func<object?> instanceFactory, Type serviceType, string? contract = null);

        /// <summary>
        /// Removes the service.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        void RemoveService(Type serviceType, string? contract = null);

        /// <summary>
        /// Removes all services.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        void RemoveAllServices(Type serviceType, string? contract = null);
    }
}
