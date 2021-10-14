// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using DynamicData;
using ReactiveMarbles.Locator;

namespace ReactiveMarbles.Mvvm
{
    /// <summary>
    /// ReactiveObject is the base object for ViewModel classes, and it
    /// implements INotifyPropertyChanged. In addition, ReactiveObject provides
    /// Changing and Changed Observables to monitor object changes.
    /// </summary>
    public class RxObject : IRxObject
    {
        private readonly Lazy<ISubject<Exception>> _thrownExceptions = new(() =>
            new ScheduledSubject<Exception>(Scheduler.Immediate, ServiceLocator.Current().GetService<ICoreRegistration>().ExceptionHandler), LazyThreadSafetyMode.PublicationOnly);

        private readonly Lazy<Notifications> _notification = new(() => new Notifications());

        /// <summary>
        /// Initializes a new instance of the <see cref="RxObject"/> class.
        /// </summary>
        protected RxObject()
        {
            Changed = Observable.Create<RxPropertyChangedEventArgs<IRxObject>>(observer =>
            {
                void Handler(object? sender, PropertyChangedEventArgs args) =>
                    observer.OnNext(new RxPropertyChangedEventArgs<IRxObject>(args.PropertyName, sender as IRxObject ?? throw new InvalidOperationException()));

                PropertyChanged += Handler;
                return Disposable.Create(() => PropertyChanged -= Handler);
            });

            Changing = Observable.Create<RxPropertyChangingEventArgs<IRxObject>>(observer =>
            {
                void Handler(object? sender, PropertyChangingEventArgs args) =>
                    observer.OnNext(new RxPropertyChangingEventArgs<IRxObject>(args.PropertyName, sender as IRxObject ?? throw new InvalidOperationException()));

                PropertyChanging += Handler;
                return Disposable.Create(() => PropertyChanging -= Handler);
            });
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc/>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <inheritdoc/>
        public IObservable<Exception> ThrownExceptions => _thrownExceptions.Value.AsObservable();

        /// <inheritdoc/>
        public IObservable<RxPropertyChangingEventArgs<IRxObject>> Changing { get; }

        /// <inheritdoc/>
        public IObservable<RxPropertyChangedEventArgs<IRxObject>> Changed { get; }

        /// <inheritdoc/>
        public bool AreChangeNotificationsEnabled() => !_notification.IsValueCreated || Interlocked.Read(ref _notification.Value.ChangeNotificationsSuppressed) == 0;

        /// <inheritdoc/>
        public bool AreChangeNotificationsDelayed() => _notification.IsValueCreated && Interlocked.Read(ref _notification.Value.ChangeNotificationsDelayed) > 0;

        /// <inheritdoc/>
        public IDisposable SuppressChangeNotifications()
        {
            Interlocked.Increment(ref _notification.Value.ChangeNotificationsSuppressed);
            return Disposable.Create(() => Interlocked.Decrement(ref _notification.Value.ChangeNotificationsSuppressed));
        }

        /// <inheritdoc/>
        public IDisposable DelayChangeNotifications()
        {
            Interlocked.Increment(ref _notification.Value.ChangeNotificationsDelayed);

            return Disposable.Create(() =>
            {
                if (Interlocked.Decrement(ref _notification.Value.ChangeNotificationsDelayed) == 0)
                {
                    foreach (var distinctEvent in DistinctEvents(_notification.Value.PropertyChangingEvents.Items.ToList()))
                    {
                        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(distinctEvent.PropertyName));
                    }

                    foreach (var distinctEvent in DistinctEvents(_notification.Value.PropertyChangedEvents.Items.ToList()))
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(distinctEvent.PropertyName));
                    }

                    _notification.Value.PropertyChangingEvents.Clear();
                    _notification.Value.PropertyChangedEvents.Clear();
                }
            });
        }

        /// <summary>
        /// Raises the property changing event.
        /// </summary>
        /// <param name="args">The argument.</param>
        protected void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            if (!AreChangeNotificationsEnabled())
            {
                return;
            }

            if (AreChangeNotificationsDelayed())
            {
                _notification.Value.PropertyChangingEvents.Add(new RxPropertyChangingEventArgs<IRxObject>(args.PropertyName, this));
                return;
            }

            try
            {
                PropertyChanging?.Invoke(this, args);
            }
            catch (Exception e)
            {
                if (!_thrownExceptions.IsValueCreated)
                {
                    throw;
                }

                _thrownExceptions.Value.OnNext(e);
            }
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="args">The argument.</param>
        protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            if (!AreChangeNotificationsEnabled())
            {
                return;
            }

            if (AreChangeNotificationsDelayed())
            {
                _notification.Value.PropertyChangedEvents.Add(new RxPropertyChangedEventArgs<IRxObject>(args.PropertyName, this));
                return;
            }

            try
            {
                PropertyChanged?.Invoke(this, args);
            }
            catch (Exception e)
            {
                if (!_thrownExceptions.IsValueCreated)
                {
                    throw;
                }

                _thrownExceptions.Value.OnNext(e);
            }
        }

        /// <summary>
        /// Raises a property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void RaisePropertyChanging([CallerMemberName] string propertyName = "") => RaisePropertyChanging(new PropertyChangingEventArgs(propertyName));

        /// <summary>
        /// Raises a property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "") => RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));

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

        /// <summary>
        /// Filter a list of change notifications, returning the last change for each PropertyName in original order.
        /// </summary>
        private static IEnumerable<TEventArgs> DistinctEvents<TEventArgs>(IList<TEventArgs> events)
            where TEventArgs : IRxPropertyEventArgs<IRxObject>
        {
            if (events.Count <= 1)
            {
                return events;
            }

            var seen = new HashSet<string>();
            var uniqueEvents = new Stack<TEventArgs>(events.Count);

            for (var i = events.Count - 1; i >= 0; i--)
            {
                var propertyName = events[i].PropertyName;
                if (propertyName is not null && seen.Add(propertyName))
                {
                    uniqueEvents.Push(events[i]);
                }
            }

            // Stack enumerates in LIFO order
            return uniqueEvents;
        }

        [SuppressMessage("StyleCop", "SA1401", Justification = "Deliberate use of private field")]
        private class Notifications
        {
            public readonly SourceList<RxPropertyChangedEventArgs<IRxObject>> PropertyChangedEvents = new();
            public readonly SourceList<RxPropertyChangingEventArgs<IRxObject>> PropertyChangingEvents = new();
            public long ChangeNotificationsDelayed;
            public long ChangeNotificationsSuppressed;
        }
    }
}
