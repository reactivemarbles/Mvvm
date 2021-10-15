// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;

namespace ReactiveMarbles.Mvvm
{
    internal class CoreRegistration : ICoreRegistration
    {
        public CoreRegistration(IScheduler mainThreadScheduler, IScheduler taskPoolScheduler, IObserver<Exception> exceptionHandler)
        {
            MainThreadScheduler = mainThreadScheduler;
            TaskPoolScheduler = taskPoolScheduler;
            ExceptionHandler = exceptionHandler;
        }

        /// <inheritdoc />
        public IScheduler MainThreadScheduler { get; }

        /// <inheritdoc />
        public IScheduler TaskPoolScheduler { get; }

        /// <inheritdoc />
        public IObserver<Exception> ExceptionHandler { get; }
    }
}
