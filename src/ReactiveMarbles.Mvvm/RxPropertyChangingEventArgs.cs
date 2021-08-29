// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace ReactiveMarbles.Core
{
    /// <summary>
    /// Event arguments for when a property is changing.
    /// </summary>
    /// <typeparam name="TSender">The sender type.</typeparam>
    public record RxPropertyChangingEventArgs<TSender>(string PropertyName, TSender Sender) : IRxPropertyEventArgs<TSender>;
}
