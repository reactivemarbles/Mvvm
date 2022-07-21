// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Event arguments for when a property is changing.
/// </summary>
/// <param name="PropertyName">The property name.</param>
/// <param name="Sender">The sender.</param>
/// <typeparam name="TSender">The sender type.</typeparam>
public record RxPropertyChangingEventArgs<TSender>(string? PropertyName, TSender Sender) : IRxPropertyEventArgs<TSender>;