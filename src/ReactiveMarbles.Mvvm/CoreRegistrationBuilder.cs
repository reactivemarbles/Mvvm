// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Builder for <see cref="CoreRegistration"/>.
/// </summary>
public class CoreRegistrationBuilder
{
    private IScheduler _mainThread = DefaultScheduler.Instance;
    private IScheduler _taskPool = TaskPoolScheduler.Default;
    private IObserver<Exception> _exceptionHandler = new DebugExceptionHandler();

    /// <summary>
    /// Returns a static builder extension to create an <see cref="CoreRegistration"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static CoreRegistrationBuilder Create() => new();

    /// <summary>
    /// Adds a main thread scheduler to the <see cref="ICoreRegistration"/>.
    /// </summary>
    /// <param name="scheduler">The main thread scheduler.</param>
    /// <returns>The builder.</returns>
    public CoreRegistrationBuilder WithMainThreadScheduler(IScheduler scheduler)
    {
        _mainThread = scheduler;
        return this;
    }

    /// <summary>
    /// Adds a <see cref="TaskPoolScheduler"/> to the core registration.
    /// </summary>
    /// <param name="scheduler">The task pool scheduler.</param>
    /// <returns>The builder.</returns>
    public CoreRegistrationBuilder WithTaskPoolScheduler(IScheduler scheduler)
    {
        _taskPool = scheduler;
        return this;
    }

    /// <summary>
    /// Adds an exception handler to the <see cref="ICoreRegistration"/>.
    /// </summary>
    /// <param name="handler">The exception handler.</param>
    /// <returns>The builder.</returns>
    public CoreRegistrationBuilder WithExceptionHandler(IObserver<Exception> handler)
    {
        _exceptionHandler = handler;
        return this;
    }

    /// <summary>
    /// Builds an <see cref="ICoreRegistration"/>.
    /// </summary>
    /// <returns>The core registration.</returns>
    public ICoreRegistration Build() => new CoreRegistration(_mainThread, _taskPool, _exceptionHandler);
}
