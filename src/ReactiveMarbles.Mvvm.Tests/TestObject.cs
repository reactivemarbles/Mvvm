// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace ReactiveMarbles.Mvvm.Tests;

/// <summary>
/// Test the thing.
/// </summary>
public class TestObject : RxObject
{
    private string _testProperty = string.Empty;

    /// <summary>
    /// Gets or sets test propety.
    /// </summary>
    public string TestProperty
    {
        get => _testProperty;
        set => RaiseAndSetIfChanged(ref _testProperty, value);
    }
}