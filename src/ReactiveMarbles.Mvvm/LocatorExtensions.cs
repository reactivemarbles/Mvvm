// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using ReactiveMarbles.Locator;

namespace ReactiveMarbles.Mvvm
{
    /// <summary>
    /// <see cref="IServiceLocator"/> extensions.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:ElementDocumentationMustNotBeCopiedAndPasted", Justification = "Builder Extensions.")]
    public static class LocatorExtensions
    {
        /// <summary>
        /// Add an exception handler.
        /// </summary>
        /// <param name="serviceLocator">The locator.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>The service locator.</returns>
        public static IServiceLocator AddExceptionHandler(this IServiceLocator serviceLocator, IObserver<Exception> exceptionHandler)
        {
            serviceLocator.AddService<IObserver<Exception>>(() => exceptionHandler);
            return serviceLocator;
        }

        /// <summary>
        /// Adds core registrations to the locator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mainthread">The main thread scheduler.</param>
        /// <param name="taskpool">The task pool scheduler.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>The service locator.</returns>
        public static IServiceLocator AddCoreRegistrations(this IServiceLocator serviceLocator, IScheduler mainthread, IScheduler taskpool, IObserver<Exception> exceptionHandler)
        {
            serviceLocator.AddService(() => new CoreRegistration(mainthread, taskpool, exceptionHandler));
            return serviceLocator;
        }

        /// <summary>
        /// Adds core registrations to the locator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="coreRegistration">The core registration.</param>
        /// <returns>The service locator.</returns>
        public static IServiceLocator AddCoreRegistrations(this IServiceLocator serviceLocator, Func<ICoreRegistration> coreRegistration)
        {
            serviceLocator.AddService(coreRegistration);
            return serviceLocator;
        }
    }
}
