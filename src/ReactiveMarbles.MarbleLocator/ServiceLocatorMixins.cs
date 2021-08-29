// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace ReactiveMarbles.MarbleLocator
{
    /// <summary>
    /// Service Locator Mixins.
    /// </summary>
    public static class ServiceLocatorMixins
    {
        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <typeparam name="T">The type used for registration.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="contract">The contract.</param>
        public static void AddService<T>(this IEditServices serviceLocator, Func<object?> instanceFactory, string? contract = null)
        {
            serviceLocator.AddService(instanceFactory, typeof(T), contract);
        }

        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="contract">The contract.</param>
        public static void AddService<TInterface, TConcrete>(this IEditServices serviceLocator, string? contract = null)
            where TConcrete : new()
        {
            serviceLocator.AddService(() => new TConcrete(), typeof(TInterface), contract);
        }

        /// <summary>
        /// Adds the singleton.
        /// </summary>
        /// <typeparam name="T">The type used for registration.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="contract">The contract.</param>
        public static void AddSingleton<T>(this IEditServices serviceLocator, object instance, string? contract = null)
        {
            serviceLocator.AddService(() => instance, typeof(T), contract);
        }

        /// <summary>
        /// Adds the singleton.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="contract">The contract.</param>
        public static void AddSingleton<TInterface, TConcrete>(this IEditServices serviceLocator, string? contract = null)
         where TConcrete : new()
        {
            var instance = new TConcrete();
            serviceLocator.AddService(() => instance, typeof(TInterface), contract);
        }

        /// <summary>
        /// Adds the lazy singleton.
        /// </summary>
        /// <typeparam name="T">The type used for registration.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="contract">The contract.</param>
        public static void AddLazySingleton<T>(this IEditServices serviceLocator, object? instance, string? contract = null)
        {
            object? InstanceFactory() => instance;
            var val = new Lazy<object?>(InstanceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            serviceLocator.AddService(() => val.Value, typeof(T), contract);
        }

        /// <summary>
        /// Adds the lazy singleton.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="contract">The contract.</param>
        public static void AddLazySingleton<TInterface, TConcrete>(this IEditServices serviceLocator, string? contract = null)
            where TConcrete : new()
        {
            object? InstanceFactory() => new TConcrete();
            var val = new Lazy<object?>(InstanceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            serviceLocator.AddService(() => val.Value, typeof(TInterface), contract);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T">The type used for registration.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>An instance of the type used for registration.</returns>
        public static T? GetService<T>(this IGetServices serviceLocator, string? contract = null) =>
            (T?)serviceLocator.GetService(typeof(T), contract);

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>An instance of the type used for registration.</returns>
        public static TConcrete? GetService<TInterface, TConcrete>(this IGetServices serviceLocator, string? contract = null) =>
            (TConcrete?)serviceLocator.GetService(typeof(TInterface), contract);
    }
}
