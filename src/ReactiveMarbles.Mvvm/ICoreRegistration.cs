// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;

namespace ReactiveMarbles.Core
{
    /// <summary>
    /// Interface that represents core Reactive Marbles registration.
    /// </summary>
    // TODO: [rlittlesii: August 21, 2021] RxApp could be Splat Regsitration
    public interface ICoreRegistration
    {
        /// <summary>
        /// Gets the main thread scheduler.
        /// </summary>
        IScheduler MainThreadScheduler { get; }

        /// <summary>
        /// Gets the task pool scheduler.
        /// </summary>
        IScheduler TaskpoolScheduler { get; }

        /// <summary>
        /// Gets the default exception handler.
        /// </summary>
        IObserver<Exception> ExceptionHandler { get; }
    }
}
