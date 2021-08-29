// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ReactiveMarbles.Mvvm
{
    /// <summary>
    /// ReactiveRecord is the base object for ViewModel classes, and it
    /// implements INotifyPropertyChanged. In addition, ReactiveRecord provides
    /// Changing and Changed Observables to monitor object changes.
    /// </summary>
    public record RxRecord : IRxObject
    {
        private readonly Lazy<Subject<Exception>> _thrownExceptions = new(() => new Subject<Exception>(), LazyThreadSafetyMode.PublicationOnly);
        private readonly Lazy<Subject<Unit>> _startOrStopDelayingChangeNotifications = new(() => new Subject<Unit>(), LazyThreadSafetyMode.PublicationOnly);
        private long _changeNotificationsDelayed;
        private long _changeNotificationsSuppressed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RxRecord"/> class.
        /// </summary>
        public RxRecord()
        {
            Changed = Observable.Create<RxPropertyChangedEventArgs<IRxObject>>(observer =>
            {
                void Handler(object sender, PropertyChangedEventArgs args) =>
                    observer.OnNext(new RxPropertyChangedEventArgs<IRxObject>(args.PropertyName, (IRxObject)sender));

                PropertyChanged += Handler;
                return Disposable.Create(() => PropertyChanged -= Handler);
            });

            Changing = Observable.Create<RxPropertyChangingEventArgs<IRxObject>>(observer =>
            {
                void Handler(object sender, PropertyChangingEventArgs args) =>
                    observer.OnNext(new RxPropertyChangingEventArgs<IRxObject>(args.PropertyName, (IRxObject)sender));

                PropertyChanging += Handler;
                return Disposable.Create(() => PropertyChanging -= Handler);
            });
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc/>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <inheritdoc/>
        public IObservable<Exception> ThrownExceptions => _thrownExceptions.Value;

        /// <inheritdoc/>
        public IObservable<RxPropertyChangingEventArgs<IRxObject>> Changing { get; }

        /// <inheritdoc/>
        public IObservable<RxPropertyChangedEventArgs<IRxObject>> Changed { get; }

        /// <inheritdoc/>
        public bool AreChangeNotificationsEnabled() => Interlocked.Read(ref _changeNotificationsSuppressed) == 0;

        /// <inheritdoc/>
        public bool AreChangeNotificationsDelayed() => Interlocked.Read(ref _changeNotificationsDelayed) > 0;

        /// <inheritdoc/>
        public IDisposable SuppressChangeNotifications()
        {
            Interlocked.Increment(ref _changeNotificationsSuppressed);
            return Disposable.Create(() => Interlocked.Decrement(ref _changeNotificationsSuppressed));
        }

        /// <inheritdoc/>
        public IDisposable DelayChangeNotifications()
        {
            if (Interlocked.Increment(ref _changeNotificationsDelayed) == 1)
            {
                if (_startOrStopDelayingChangeNotifications.IsValueCreated)
                {
                    _startOrStopDelayingChangeNotifications.Value.OnNext(Unit.Default);
                }
            }

            return Disposable.Create(() =>
            {
                if (Interlocked.Decrement(ref _changeNotificationsDelayed) == 0)
                {
                    if (_startOrStopDelayingChangeNotifications.IsValueCreated)
                    {
                        _startOrStopDelayingChangeNotifications.Value.OnNext(Unit.Default);
                    }
                }
            });
        }

        /// <summary>
        /// Raises the property changing event.
        /// </summary>
        /// <param name="args">The argument.</param>
        protected void RaisePropertyChanging(PropertyChangingEventArgs args) => PropertyChanging?.Invoke(this, args);

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="args">The argument.</param>
        protected void RaisePropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

        /// <summary>
        /// Raises a property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void RaisePropertyChanging([CallerMemberName] string propertyName = "")
        {
            if (!AreChangeNotificationsEnabled())
            {
                return;
            }

            try
            {
                RaisePropertyChanging(new PropertyChangingEventArgs(propertyName));
            }
            catch (Exception e)
            {
                if (_thrownExceptions.IsValueCreated)
                {
                    _thrownExceptions.Value.OnNext(e);
                }
            }
        }

        /// <summary>
        /// Raises a property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (!AreChangeNotificationsEnabled())
            {
                return;
            }

            try
            {
                RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception e)
            {
                if (_thrownExceptions.IsValueCreated)
                {
                    _thrownExceptions.Value.OnNext(e);
                }
            }
        }

        /// <summary>
        /// RaiseAndSetIfChanged fully implements a Setter for a read-write
        /// property on a ReactiveObject, using CallerMemberName to raise the notification
        /// and the ref to the backing field to set the property.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="backingField">A Reference to the backing field for this
        /// property.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">The name of the property, usually
        /// automatically provided through the CallerMemberName attribute.</param>
        protected void RaiseAndSetIfChanged<T>(
            ref T backingField,
            T newValue,
            [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                return;
            }

            RaisePropertyChanging(propertyName);
            backingField = newValue;
            RaisePropertyChanged(propertyName);
        }
    }
}
