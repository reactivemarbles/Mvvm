// Copyright (c) 2021 Reactive Marbles. All rights reserved.
// The Reactive Marbles licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ReactiveMarbles.Core
{
    /// <summary>
    /// A reactive object is a interface for ViewModels which will expose
    /// logging, and notify when properties are either changing or changed.
    /// The primary use of this interface is to allow external classes such as
    /// the ObservableAsPropertyHelper to trigger these events inside the ViewModel.
    /// </summary>
    public interface IRxObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Gets internal thrown exceptions.
        /// </summary>
        protected internal Lazy<ISubject<Exception>> ThrownExceptionsInternal { get; }

        /// <summary>
        /// Gets a observable for when an exception is thrown.
        /// </summary>
        IObservable<Exception> ThrownExceptions { get; }

        /// <summary>
        /// Gets the change state of the object.
        /// </summary>
        protected internal Lazy<ChangeState> State { get; }

        /// <summary>
        /// Raises a property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        public virtual void RaisePropertyChanging([CallerMemberName] string propertyName = "")
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
                if (ThrownExceptionsInternal.IsValueCreated)
                {
                    ThrownExceptionsInternal.Value.OnNext(e);
                }
            }
        }

        /// <summary>
        /// Raises a property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
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
                if (ThrownExceptionsInternal.IsValueCreated)
                {
                    ThrownExceptionsInternal.Value.OnNext(e);
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
        void RaiseAndSetIfChanged<T>(
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
        /// Raise a property is changing event.
        /// </summary>
        /// <param name="args">The arguments with details about the property that is changing.</param>
        void RaisePropertyChanging(PropertyChangingEventArgs args);

        /// <summary>
        /// Raise a property has changed event.
        /// </summary>
        /// <param name="args">The arguments with details about the property that has changed.</param>
        void RaisePropertyChanged(PropertyChangedEventArgs args);

        /// <summary>
        /// Gets a value indicating whether the change notifications are enabled.
        /// </summary>
        /// <returns>A truth value.</returns>
        public bool AreChangeNotificationsEnabled() => !State.IsValueCreated || Interlocked.Read(ref State.Value.ChangeNotificationsSuppressed) == 0;

        /// <summary>
        /// Gets a value indicating whether the change notifications are delayed.
        /// </summary>
        /// <returns>A truth value.</returns>
        public bool AreChangeNotificationsDelayed() => State.IsValueCreated && Interlocked.Read(ref State.Value.ChangeNotificationsDelayed) > 0;

        /// <summary>
        /// When this method is called, an object will not fire change
        /// notifications (neither traditional nor Observable notifications)
        /// until the return value is disposed.
        /// If this method is called multiple times it will reference count
        /// and not perform notification until all values returned are disposed.
        /// </summary>
        /// <returns>An object that, when disposed, re-enables change
        /// notifications.</returns>
        public IDisposable SuppressChangeNotifications()
        {
            Interlocked.Increment(ref State.Value.ChangeNotificationsSuppressed);
            return Disposable.Create(() => Interlocked.Decrement(ref State.Value.ChangeNotificationsSuppressed));
        }

        /// <summary>
        /// When this method is called, an object will not dispatch change
        /// Observable notifications until the return value is disposed.
        /// When the Disposable it will dispatched all queued notifications.
        /// If this method is called multiple times it will reference count
        /// and not perform notification until all values returned are disposed.
        /// </summary>
        /// <returns>An object that, when disposed, re-enables Observable change
        /// notifications.</returns>
        public IDisposable DelayChangeNotifications()
        {
            if (Interlocked.Increment(ref State.Value.ChangeNotificationsDelayed) == 1)
            {
                if (State.IsValueCreated)
                {
                    State.Value.StartOrStopDelayingChangeNotifications.OnNext(Unit.Default);
                }
            }

            return Disposable.Create(() =>
            {
                if (Interlocked.Decrement(ref State.Value.ChangeNotificationsDelayed) == 0)
                {
                    if (State.IsValueCreated)
                    {
                        State.Value.StartOrStopDelayingChangeNotifications.OnNext(Unit.Default);
                    }
                }
            });
        }

        /// <summary>
        /// Represents the changed state of an <see cref="IRxObject"/>.
        /// </summary>
        [SuppressMessage("SA1401", "SA1401", Justification = "Deliberate Field usage for default interface implementation.")]
        protected internal class ChangeState
        {
            /// <summary>
            /// Gets the change notification delays.
            /// </summary>
            public long ChangeNotificationsDelayed;

            /// <summary>
            /// Gets the change notification suppression.
            /// </summary>
            public long ChangeNotificationsSuppressed;

            /// <summary>
            /// Gets the delayed change notification subject.
            /// </summary>
            public Subject<Unit> StartOrStopDelayingChangeNotifications { get; } = new Subject<Unit>();
        }
    }
}
