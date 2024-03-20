// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// A reactive object is a interface for ViewModels which will expose
/// logging, and notify when properties are either changing or changed.
/// The primary use of this interface is to allow external classes such as
/// the ObservableAsPropertyHelper to trigger these events inside the ViewModel.
/// </summary>
public interface IRxObject : INotifyPropertyChanged, INotifyPropertyChanging, IThrownExceptions
{
    /// <summary>
    /// Gets an observable that fires *before* a property is about to
    /// be changed. Note that this should not fire duplicate change notifications if a
    /// property is set to the same value multiple times.
    /// </summary>
    IObservable<RxPropertyChangingEventArgs<IRxObject>> Changing { get; }

    /// <summary>
    /// Gets an Observable that fires *after* a property has changed.
    /// Note that this should not fire duplicate change notifications if a
    /// property is set to the same value multiple times.
    /// </summary>
    IObservable<RxPropertyChangedEventArgs<IRxObject>> Changed { get; }

    /// <summary>
    /// When this method is called, an object will not fire change
    /// notifications (neither traditional nor Observable notifications)
    /// until the return value is disposed.
    /// </summary>
    /// <returns>An object that, when disposed, re-enables change
    /// notifications.</returns>
    IDisposable SuppressChangeNotifications();

    /// <summary>
    /// Determines if change notifications are enabled or not.
    /// </summary>
    /// <returns>A value indicating whether change notifications are enabled.</returns>
    bool AreChangeNotificationsEnabled();

    /// <summary>
    /// Gets a value indicating whether the change notifications are delayed.
    /// </summary>
    /// <returns>A truth value.</returns>
    bool AreChangeNotificationsDelayed();

    /// <summary>
    /// Delays notifications until the return IDisposable is disposed.
    /// </summary>
    /// <returns>A disposable which when disposed will send delayed notifications.</returns>
    IDisposable DelayChangeNotifications();
}
