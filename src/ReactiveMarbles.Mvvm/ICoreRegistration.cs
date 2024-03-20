// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Interface that represents core Reactive Marbles registration.
/// </summary>
public interface ICoreRegistration
{
    /// <summary>
    /// Gets the main thread scheduler.
    /// </summary>
    IScheduler MainThreadScheduler { get; }

    /// <summary>
    /// Gets the task pool scheduler.
    /// </summary>
    IScheduler TaskPoolScheduler { get; }

    /// <summary>
    /// Gets the default exception handler.
    /// </summary>
    IObserver<Exception> ExceptionHandler { get; }
}