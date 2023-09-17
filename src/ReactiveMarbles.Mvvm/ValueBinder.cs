// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveMarbles.Extensions;

namespace ReactiveMarbles.Mvvm;

/// <summary>
///  <see cref="ValueBinder{T}"/> is a class to help produce a bindable value from an observable sequence. This property is
/// read-only and will fire change notifications.  Using the <see cref="AsValueExtensions"/> methods allows easy creation
/// from an observable sequence.
/// </summary>
/// <typeparam name="T">The value type.</typeparam>
public sealed class ValueBinder<T> : IValueBinder<T>
{
    private readonly CompositeDisposable _disposable = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueBinder{T}"/> class.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanging">The on changing delegate.</param>
    /// <param name="onChanged">The on changed delegate.</param>
    /// <param name="scheduler">The thread scheduler.</param>
    /// <param name="initialValue">The function that provides an initial value.</param>
    public ValueBinder(
        IObservable<T> source,
        Action<T>? onChanging,
        Action<T> onChanged,
        IScheduler? scheduler,
        Func<T> initialValue)
    {
        var subject = new ProxyScheduledSubject<T>(scheduler ?? CurrentThreadScheduler.Instance);
        Value = initialValue.Invoke();
        _ = subject
            .Subscribe(value =>
            {
                onChanging?.Invoke(value);
                Value = value;
                onChanged.Invoke(value);
            }).DisposeWith(_disposable);
        _ = source.DistinctUntilChanged().StartWith(Value).Subscribe(subject).DisposeWith(_disposable);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueBinder{T}"/> class.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">The on changed delegate.</param>
    /// <param name="initialValue">The function that provides an initial value.</param>
    public ValueBinder(IObservable<T> source, Action<T> onChanged, Func<T> initialValue)
        : this(source, null, onChanged, null, initialValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueBinder{T}"/> class.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanging">The on changing delegate.</param>
    /// <param name="onChanged">The on changed delegate.</param>
    /// <param name="initialValue">The function that provides an initial value.</param>
    public ValueBinder(IObservable<T> source, Action<T> onChanging, Action<T> onChanged, Func<T> initialValue)
        : this(source, onChanging, onChanged, null, initialValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueBinder{T}"/> class.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">The on changed.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <param name="initialValue">The function that provides an initial value.</param>
    public ValueBinder(IObservable<T> source, Action<T> onChanged, IScheduler scheduler, Func<T> initialValue)
        : this(source, null, onChanged, scheduler, initialValue)
    {
    }

    /// <summary>
    /// Gets the latest value.
    /// </summary>
    public T Value { get; private set; }

    /// <inheritdoc />
    public void Dispose() => _disposable.Dispose();
}
