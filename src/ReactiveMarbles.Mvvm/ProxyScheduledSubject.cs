// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// A subject which dispatches all its events on the specified Scheduler.
/// </summary>
/// <typeparam name="T">The type of item being dispatched by the Subject.</typeparam>
public class ProxyScheduledSubject<T> : ISubject<T>, IDisposable
{
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly IObserver<T> _defaultObserver;
    private readonly IScheduler _scheduler;
    private readonly ISubject<T> _subject;
    private int _observerRefCount;
    private IDisposable _defaultObserverSub = Disposable.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyScheduledSubject{T}"/> class.
    /// </summary>
    /// <param name="scheduler">The scheduler where to dispatch items to.</param>
    /// <param name="defaultObserver">A optional default observer where notifications will be sent.</param>
    /// <param name="defaultSubject">A optional default subject which this Subject will wrap.</param>
    public ProxyScheduledSubject(IScheduler scheduler, IObserver<T>? defaultObserver = null, ISubject<T>? defaultSubject = null)
    {
        _scheduler = scheduler;
        _defaultObserver = defaultObserver ?? new Subject<T>();
        _subject = defaultSubject ?? new Subject<T>();

        if (defaultObserver is not null)
        {
            _defaultObserverSub = _subject.ObserveOn(_scheduler).Subscribe(_defaultObserver).DisposeWith(_disposables);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public void OnCompleted() => _subject.OnCompleted();

    /// <inheritdoc/>
    public void OnError(Exception error) => _subject.OnError(error);

    /// <inheritdoc/>
    public void OnNext(T value) => _subject.OnNext(value);

    /// <inheritdoc/>
    public IDisposable Subscribe(IObserver<T> observer)
    {
        Interlocked.Exchange(ref _defaultObserverSub, Disposable.Empty).Dispose();

        Interlocked.Increment(ref _observerRefCount);

        return new CompositeDisposable(
            _subject.ObserveOn(_scheduler).Subscribe(observer),
            Disposable.Create(() =>
            {
                if (Interlocked.Decrement(ref _observerRefCount) <= 0)
                {
                    _defaultObserverSub = _subject.ObserveOn(_scheduler).Subscribe(_defaultObserver);
                }
            }));
    }

    /// <summary>
    /// Disposes of any managed resources in our class.
    /// </summary>
    /// <param name="isDisposing">If we are being called by the IDisposable method.</param>
    protected virtual void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            if (_subject is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _disposables.Dispose();
            _defaultObserverSub.Dispose();
        }
    }
}