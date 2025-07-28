// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;

namespace ReactiveMarbles.Mvvm;

internal class CoreRegistration(IScheduler mainThreadScheduler, IScheduler taskPoolScheduler, IObserver<Exception> exceptionHandler) : ICoreRegistration
{
    /// <inheritdoc />
    public IScheduler MainThreadScheduler { get; } = mainThreadScheduler;

    /// <inheritdoc />
    public IScheduler TaskPoolScheduler { get; } = taskPoolScheduler;

    /// <inheritdoc />
    public IObserver<Exception> ExceptionHandler { get; } = exceptionHandler;
}
