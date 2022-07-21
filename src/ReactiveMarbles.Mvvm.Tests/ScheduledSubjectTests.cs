// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;

using FluentAssertions;

using Microsoft.Reactive.Testing;

using Xunit;

namespace ReactiveMarbles.Mvvm.Tests;

/// <summary>
/// ScheduledSubject Tests.
/// </summary>
public class ScheduledSubjectTests
{
    /// <summary>
    /// Tests ScheduledSubject.
    /// </summary>
    [Fact]
    public void GivenTestScheduler_WhenSubscribed_ThenReturnResult()
    {
        // Given
        Unit? result = null;
        TestScheduler? scheduler = new();
        ProxyScheduledSubject<Unit>? sut = new(scheduler);
        _ = sut.Subscribe(actual => result = actual);

        // When
        sut.OnNext(Unit.Default);
        scheduler.Start();

        // Then
        _ = result.HasValue.Should().BeTrue();
    }
}
