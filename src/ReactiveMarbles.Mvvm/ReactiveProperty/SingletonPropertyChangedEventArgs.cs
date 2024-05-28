// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace ReactiveMarbles.Mvvm;

internal static class SingletonPropertyChangedEventArgs
{
    public static readonly PropertyChangedEventArgs Value = new(nameof(Value));
    public static readonly PropertyChangedEventArgs HasErrors = new(nameof(INotifyDataErrorInfo.HasErrors));
    public static readonly PropertyChangedEventArgs ErrorMessage = new(nameof(ErrorMessage));
}
