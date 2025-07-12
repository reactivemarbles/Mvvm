// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Represents a typed value for binding to UI elements.
/// </summary>
/// <typeparam name="T">The value type.</typeparam>
public interface IValueBinder<T> : IDisposable
{
    /// <summary>
    /// Gets the latest value.
    /// </summary>
    T Value { get; }
}