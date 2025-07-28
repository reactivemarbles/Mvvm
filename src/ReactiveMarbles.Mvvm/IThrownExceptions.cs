// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// <para>
/// This interface is implemented by Rx objects which are given
/// IObservables as input - when the input IObservables OnError, instead of
/// disabling the Rx object, we catch the IObservable and pipe it into
/// this property.
/// </para>
/// <para>
/// Normally this IObservable is implemented with a ScheduledSubject whose
/// default Observer is CoreRegistration.DefaultExceptionHandler - this means, that if
/// you aren't listening to ThrownExceptions and one appears, the exception
/// will appear on the UI thread and crash the application.
/// </para>
/// </summary>
public interface IThrownExceptions
{
    /// <summary>
    /// Gets a observable which will fire whenever an exception would normally terminate.
    /// internal state.
    /// </summary>
    IObservable<Exception> ThrownExceptions { get; }
}
