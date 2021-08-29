// Copyright (c) 2021 Reactive Marbles. All rights reserved.
// The Reactive Marbles licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;

namespace ReactiveMarbles.Core
{
    /// <summary>
    /// ISuspensionDriver represents a class that can load/save state to persistent
    /// storage. Most platforms have a basic implementation of this class, but you
    /// probably want to write your own.
    /// </summary>
    public interface IStateHandler
    {
        /// <summary>
        /// Loads the application state from persistent storage.
        /// </summary>
        /// <returns>An object observable.</returns>
        IObservable<object> LoadState();

        /// <summary>
        /// Saves the application state to disk.
        /// </summary>
        /// <param name="state">The application state.</param>
        /// <returns>A completed observable.</returns>
        IObservable<Unit> SaveState(object state);

        /// <summary>
        /// Invalidates the application state (i.e. deletes it from disk).
        /// </summary>
        /// <returns>A completed observable.</returns>
        IObservable<Unit> InvalidateState();
    }
}
