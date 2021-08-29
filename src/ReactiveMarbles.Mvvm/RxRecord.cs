// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace ReactiveMarbles.Core
{
    /// <summary>
    /// ReactiveRecord is the base object for ViewModel classes, and it
    /// implements INotifyPropertyChanged. In addition, ReactiveRecord provides
    /// Changing and Changed Observables to monitor object changes.
    /// </summary>
    public record RxRecord : IRxObject
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc/>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <inheritdoc/>
        Lazy<ISubject<Exception>> IRxObject.ThrownExceptionsInternal { get; } =
            new(() => new Subject<Exception>());

        /// <inheritdoc/>
        public IObservable<Exception> ThrownExceptions => ((IRxObject)this).ThrownExceptionsInternal.Value;

        /// <inheritdoc/>
        Lazy<IRxObject.ChangeState> IRxObject.State { get; } = new(LazyThreadSafetyMode.PublicationOnly);

        /// <inheritdoc/>
        void IRxObject.RaisePropertyChanging(PropertyChangingEventArgs args) =>
            PropertyChanging?.Invoke(this, args);

        /// <inheritdoc />
        void IRxObject.RaisePropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

        /// <inheritdoc/>
        public bool AreChangeNotificationsEnabled() => ((IRxObject)this).AreChangeNotificationsEnabled();

        /// <inheritdoc/>
        public bool AreChangeNotificationsDelayed() => ((IRxObject)this).AreChangeNotificationsDelayed();

        /// <inheritdoc/>
        public IDisposable DelayChangeNotifications() => ((IRxObject)this).DelayChangeNotifications();

        /// <inheritdoc/>
        public IDisposable SuppressChangeNotifications() => ((IRxObject)this).SuppressChangeNotifications();

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
        // ReSharper disable once MemberHidesInterfaceMemberWithDefaultImplementation
        protected void RaiseAndSetIfChanged<T>(
            ref T backingField,
            T newValue,
            [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is not null)
            {
                ((IRxObject)this).RaiseAndSetIfChanged(ref backingField, newValue, propertyName);
            }
        }

        /// <summary>
        /// Use this method in your ReactiveObject classes when creating custom
        /// properties where raiseAndSetIfChanged doesn't suffice.
        /// </summary>
        /// <param name="propertyName">
        /// A string representing the name of the property that has been changed.
        /// Leave <c>null</c> to let the runtime set to caller member name.
        /// </param>
        protected void RaisePropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is not null)
            {
                ((IRxObject)this).RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Use this method in your ReactiveObject classes when creating custom
        /// properties where raiseAndSetIfChanged doesn't suffice.
        /// </summary>
        /// <param name="propertyName">
        /// A string representing the name of the property that has been changed.
        /// Leave <c>null</c> to let the runtime set to caller member name.
        /// </param>
        protected void RaisePropertyChanging(
            [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is not null)
            {
                ((IRxObject)this).RaisePropertyChanging(propertyName);
            }
        }
    }
}
