// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using FluentAssertions;

using Microsoft.Reactive.Testing;

using ReactiveMarbles.PropertyChanged;

using Xunit;

namespace ReactiveMarbles.Mvvm.Tests;

/// <summary>
/// Tests for the <see cref="AsValueExtensions"/>.
/// </summary>
public class AsLazyValueExtensionsTests
{
    /// <summary>
    /// Tests the default value.
    /// </summary>
    [Fact]
    public void GivenNoChanges_WhenAsValue_ThenFullNameIsEmpty()
    {
        // Given, When
        AsLazyValueTestObject? sut = new();

        // Then
        _ = sut.FullName.Should().BeNullOrEmpty();
    }

    /// <summary>
    /// Tests the property is produced from the sequence.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType = typeof(AsValueTestData))]
    public void GivenSequence_WhenAsValue_ThenPropertyProduced(string first, string last)
    {
        // Given
        AsLazyValueTestObject? sut = new()
        {
            // When
            FirstName = first,
            LastName = last,
        };

        // Then
        _ = sut.FullName.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the property is produced from the sequence.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType = typeof(AsValueTestData))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "Deliberate")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Deliberate")]
    public void GivenFirstName_WhenAsValue_ThenPropertyProduced(string first, string last)
    {
        // Given
        AsLazyValueTestObject? sut = new()
        {
            // When
            FirstName = first,
        };

        // Then
        _ = sut.FullName.Should().Be(first);
    }

    /// <summary>
    /// Tests the property is produced from the sequence.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType = typeof(AsValueTestData))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "Deliberate")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Deliberate")]
    public void GivenLastName_WhenAsValue_ThenPropertyProduced(string first, string last)
    {
        // Given
        AsLazyValueTestObject? sut = new()
        {
            // When
            LastName = last,
        };

        // Then
        _ = sut.FullName.Should().Be(last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType = typeof(AsValueTestData))]
    public void GivenOnChanged_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        AsLazyValueTestObject? testObject = new();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(onChanged: _ => { });

        // When
        testObject.FirstName = first;
        testObject.LastName = last;

        // Then
        _ = sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType = typeof(AsValueTestData))]
    public void GivenOnChangedAndInitialValue_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        AsLazyValueTestObject? testObject = new();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(onChanged: _ => { }, initialValue: () => string.Empty);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;

        // Then
        _ = sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType = typeof(AsValueTestData))]
    public void GivenOnChangedAndOnChangingAndInitialValue_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        AsLazyValueTestObject? testObject = new();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(onChanging: _ => { }, onChanged: _ => { }, initialValue: () => string.Empty);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;

        // Then
        _ = sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType = typeof(AsValueTestData))]
    public void GivenOnChangedAndOnChangingAndSchedulerAndInitialValue_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        const string start = "start";
        TestScheduler? testScheduler = new();
        AsLazyValueTestObject? testObject = new();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(onChanged: _ => { }, testScheduler, () => start);

        _ = sut.Value.Should().Be(start);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;
        testScheduler.Start();

        // Then
        _ = sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType = typeof(AsValueTestData))]
    public void GivenAllParameters_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        const string start = "start";
        TestScheduler? testScheduler = new();
        AsLazyValueTestObject? testObject = new();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(_ => { }, _ => { }, testScheduler, () => start);

        _ = sut.Value.Should().Be(start);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;
        testScheduler.Start();

        // Then
        _ = sut.Value.Should().Be(first + last);
    }
}
