// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveMarbles.MarbleLocator
{
    /// <summary>
    /// Service Locator.
    /// </summary>
    /// <seealso cref="System.IServiceProvider" />
    public sealed class ServiceLocator : IServiceLocator
    {
        private static ServiceLocator? _current;
        private readonly Dictionary<(Type ServiceType, string? Contract), List<Func<object?>>> _store;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        private ServiceLocator() => _store = new();

        /// <summary>
        /// Currents this instance.
        /// </summary>
        /// <returns>Service Locator instance.</returns>
        public static ServiceLocator Current() => _current ??= new();

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType">serviceType</paramref>.   -or-  null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.
        /// </returns>
        public object GetService(Type serviceType) => 
            GetServices(serviceType).LastOrDefault()!;

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType">serviceType</paramref>.   -or-  null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.
        /// </returns>
        public object GetService(Type serviceType, string? contract) =>
            GetServices(serviceType, contract).LastOrDefault()!;

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>
        /// A service object[] of type <paramref name="serviceType">serviceType</paramref>.   -or-  null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.
        /// </returns>
        public object[] GetServices(Type serviceType, string? contract = null) =>
            (_store.TryGetValue((serviceType, contract ?? string.Empty), out var funcValue) ? funcValue?.Select(x => x()).ToArray() : default)!;

        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        public void AddService(Func<object?> instanceFactory, Type serviceType, string? contract = null)
        {
            var key = (serviceType, contract ?? string.Empty);
            if (!HasService(serviceType, contract))
            {
                _store.Add(key, new());
            }

            _store[(serviceType, contract)].Add(instanceFactory);
        }

        /// <summary>
        /// Determines whether the specified service type has service.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>
        ///   <c>true</c> if the specified service type has service; otherwise, <c>false</c>.
        /// </returns>
        public bool HasService(Type serviceType, string? contract = null) => 
            _store.ContainsKey((serviceType, contract ?? string.Empty));

        /// <summary>
        /// Removes the service.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        public void RemoveService(Type serviceType, string? contract = null)
        {
            var key = (serviceType, contract ?? string.Empty);
            if (!HasService(serviceType, contract))
            {
                return;
            }

            var list = _store[key];
            list.RemoveAt(list.Count - 1);
            if (list.Count == 0)
            {
                _store.Remove(key);
            }
        }

        /// <summary>
        /// Removes all services.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="contract">The contract.</param>
        public void RemoveAllServices(Type serviceType, string? contract = null)
        {
            var key = (serviceType, contract ?? string.Empty);
            if (!HasService(serviceType, contract))
            {
                return;
            }

            _store.Remove(key);
        }
    }
}
