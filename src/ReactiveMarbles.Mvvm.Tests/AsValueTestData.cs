// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace ReactiveMarbles.Mvvm.Tests;

/// <summary>
/// Represents test data for As Value Extension tests.
/// </summary>
public static class AsValueTestData
{
    /// <summary>
    /// Gets test data.
    /// </summary>
    public static IEnumerable<object[]> Data =>
        [
            ["Leeroy", "Jenkins"],
            ["James", "Kirk"],
            ["Major", "Payne"],
        ];
}
