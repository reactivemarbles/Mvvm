// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;

namespace ReactiveMarbles.Mvvm.Tests;
internal class TestRegistrations : ICoreRegistration
{
    public TestRegistrations()
    {
        MainThreadScheduler = new TestScheduler();
        TaskpoolScheduler = new TestScheduler();
        ExceptionHandler = new DebugExceptionHandler();
    }

    /// <inheritdoc />
    public IScheduler MainThreadScheduler { get; }

    /// <inheritdoc />
    public IScheduler TaskpoolScheduler { get; }

    /// <inheritdoc />
    public IObserver<Exception> ExceptionHandler { get; }
}
