// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using DynamicData;

namespace ReactiveMarbles.Core
{
    /// <summary>
    /// ReactiveObject is the base object for ViewModel classes, and it
    /// implements INotifyPropertyChanged. In addition, ReactiveObject provides
    /// Changing and Changed Observables to monitor object changes.
    /// </summary>
    public class RxObject : IRxObject
    {
        private readonly SourceList<RxPropertyChangedEventArgs<IRxObject>> _propertyChangedEvents = new();
        private readonly SourceList<RxPropertyChangingEventArgs<IRxObject>> _propertyChangingEvents = new();
        private readonly Lazy<Subject<Exception>> _thrownExceptions = new(() => new Subject<Exception>(), LazyThreadSafetyMode.PublicationOnly);
        private long _changeNotificationsDelayed;
        private long _changeNotificationsSuppressed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RxObject"/> class.
        /// </summary>
        protected RxObject()
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
            Interlocked.Increment(ref _changeNotificationsDelayed);

            return Disposable.Create(() =>
            {
                if (Interlocked.Decrement(ref _changeNotificationsDelayed) == 0)
                {
                    foreach (var distinctEvent in DistinctEvents(_propertyChangingEvents.Items.ToList()))
                    {
                        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(distinctEvent.PropertyName));
                    }

                    foreach (var distinctEvent in DistinctEvents(_propertyChangedEvents.Items.ToList()))
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(distinctEvent.PropertyName));
                    }

                    _propertyChangedEvents.Clear();
                    _propertyChangingEvents.Clear();
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
                _propertyChangingEvents.Add(new RxPropertyChangingEventArgs<IRxObject>(args.PropertyName, this));
                return;
            }

            try
            {
                PropertyChanging?.Invoke(this, args);
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
                _propertyChangedEvents.Add(new RxPropertyChangedEventArgs<IRxObject>(args.PropertyName, this));
                return;
            }

            try
            {
                PropertyChanged?.Invoke(this, args);
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
    }
}
