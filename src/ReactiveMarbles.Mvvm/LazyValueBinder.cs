// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using ReactiveMarbles.Extensions;

namespace ReactiveMarbles.Mvvm;

/// <summary>
///  <see cref="LazyValueBinder{T}"/> is a class to help produce a bindable value from an observable sequence. This class
/// will not produce a value until an observer subscribes to the value.  This property is read-only and will fire change
/// notifications.  Using the <see cref="AsValueExtensions"/> methods allows easy creation from an observable sequence.
/// </summary>
/// <typeparam name="T">The value type.</typeparam>
internal sealed class LazyValueBinder<T> : IValueBinder<T>
{
    private readonly CompositeDisposable _disposable = [];
    private readonly Func<T> _initialValue;
    private Action? _lazy;
    private T? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyValueBinder{T}"/> class.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanging">The on changing delegate.</param>
    /// <param name="onChanged">The on changed delegate.</param>
    /// <param name="scheduler">The thread scheduler.</param>
    /// <param name="initialValue">The function that provides an initial value.</param>
    public LazyValueBinder(
        IObservable<T> source,
        Action<T>? onChanging,
        Action<T> onChanged,
        IScheduler? scheduler,
        Func<T> initialValue)
    {
        _initialValue = initialValue;
        var subject = new ProxyScheduledSubject<T>(scheduler ?? CurrentThreadScheduler.Instance);
        _lazy = () =>
        {
            _value = _initialValue.Invoke();
            _ = subject
                .Subscribe(value =>
                {
                    onChanging?.Invoke(value);
                    _value = value;
                    onChanged.Invoke(value);
                }).DisposeWith(_disposable);
            _ = source.DistinctUntilChanged().StartWith(_value).Subscribe(subject).DisposeWith(_disposable);
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyValueBinder{T}"/> class.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">The on changed delegate.</param>
    /// <param name="initialValue">The function that provides an initial value.</param>
    public LazyValueBinder(IObservable<T> source, Action<T> onChanged, Func<T> initialValue)
        : this(source, null, onChanged, null, initialValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyValueBinder{T}"/> class.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanging">The on changing delegate.</param>
    /// <param name="onChanged">The on changed delegate.</param>
    /// <param name="initialValue">The function that provides an initial value.</param>
    public LazyValueBinder(IObservable<T> source, Action<T> onChanging, Action<T> onChanged, Func<T> initialValue)
        : this(source, onChanging, onChanged, null, initialValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyValueBinder{T}"/> class.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">The on changed delegate.</param>
    /// <param name="scheduler">The thread scheduler.</param>
    /// <param name="initialValue">The function that provides an initial value.</param>
    public LazyValueBinder(IObservable<T> source, Action<T> onChanged, IScheduler scheduler, Func<T> initialValue)
        : this(source, null, onChanged, scheduler, initialValue)
    {
    }

    /// <summary>
    /// Gets the latest value.
    /// </summary>
    public T Value
    {
        get
        {
            Interlocked.Exchange(ref _lazy, null)?.Invoke();
            return _value ?? _initialValue.Invoke();
        }
    }

    /// <inheritdoc />
    public void Dispose() => _disposable.Dispose();
}
