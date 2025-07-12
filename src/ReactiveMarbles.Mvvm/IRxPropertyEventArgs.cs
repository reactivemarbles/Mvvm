// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Interface representing property changing and changed event arguments.
/// </summary>
/// <typeparam name="TSender">The sender type.</typeparam>
public interface IRxPropertyEventArgs<out TSender>
{
    /// <summary>
    /// Gets the property name that changed.
    /// </summary>
    string? PropertyName { get; }

    /// <summary>
    /// Gets the sender that changed.
    /// </summary>
    TSender Sender { get; }
}