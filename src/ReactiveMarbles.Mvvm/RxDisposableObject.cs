// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Rx object that extends the <see cref="IDisposable"/> interface.
/// </summary>
public class RxDisposableObject : RxObject, IDisposable
{
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
    }
}