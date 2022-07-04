// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
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
        var scheduler = new TestScheduler();
        var sut = new ScheduledSubject<Unit>(scheduler);
        sut.Subscribe(actual => result = actual);

        // When
        sut.OnNext(Unit.Default);
        scheduler.Start();

        // Then
        result.HasValue.Should().BeTrue();
    }
}
