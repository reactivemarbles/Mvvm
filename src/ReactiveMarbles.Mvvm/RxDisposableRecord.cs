// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Disposables;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Rx object that extends the <see cref="IDisposable"/> interface.
/// </summary>
public record RxDisposableRecord : RxRecord, ICancelable
{
    /// <summary>
    /// Gets a value indicating whether this instance is disposed.
    /// </summary>
    /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
    public bool IsDisposed => Disposables.IsDisposed;

    /// <summary>
    /// Gets the disposables.
    /// </summary>
    /// <value>
    /// The disposables.
    /// </value>
    protected CompositeDisposable Disposables { get; } = [];

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the resources.
    /// </summary>
    /// <param name="disposing">Disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && !IsDisposed)
        {
            Disposables.Dispose();
        }
    }
}
