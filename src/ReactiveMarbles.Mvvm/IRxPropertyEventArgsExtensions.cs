// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace ReactiveMarbles.Mvvm;

internal static class IRxPropertyEventArgsExtensions
{
    /// <summary>
    /// Filter a list of change notifications, returning the last change for each PropertyName in original order.
    /// </summary>
    public static IEnumerable<TEventArgs> DistinctEvents<TEventArgs>(this IEnumerable<TEventArgs> events)
        where TEventArgs : IRxPropertyEventArgs<IRxObject>
    {
        var eventArgsList = events.ToList();
        if (eventArgsList.Count <= 1)
        {
            return eventArgsList;
        }

        var seen = new HashSet<string>();
        var uniqueEvents = new Stack<TEventArgs>(eventArgsList.Count);

        for (var i = eventArgsList.Count - 1; i >= 0; i--)
        {
            var propertyName = eventArgsList[i].PropertyName;
            if (propertyName is not null && seen.Add(propertyName))
            {
                uniqueEvents.Push(eventArgsList[i]);
            }
        }

        // Stack enumerates in LIFO order
        return uniqueEvents;
    }
}
