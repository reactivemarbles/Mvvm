// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Debug exception handler.
/// This is the default exception handler.
/// </summary>
public class DebugExceptionHandler : IObserver<Exception>
{
    /// <inheritdoc />
    public void OnCompleted()
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }
    }

    /// <inheritdoc />
    public void OnNext(Exception value)
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }
    }
}
