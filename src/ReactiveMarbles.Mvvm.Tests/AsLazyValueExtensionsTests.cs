// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
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
        var sut = new AsLazyValueTestObject();

        // Then
        sut.FullName.Should().BeNullOrEmpty();
    }

    /// <summary>
    /// Tests the property is produced from the sequence.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType=typeof(AsValueTestData))]
    public void GivenSequence_WhenAsValue_ThenPropertyProduced(string first, string last)
    {
        // Given
        var sut = new AsLazyValueTestObject();

        // When
        sut.FirstName = first;
        sut.LastName = last;

        // Then
        sut.FullName.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the property is produced from the sequence.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType=typeof(AsValueTestData))]
    public void GivenFirstName_WhenAsValue_ThenPropertyProduced(string first, string last)
    {
        // Given
        var sut = new AsLazyValueTestObject();

        // When
        sut.FirstName = first;

        // Then
        sut.FullName.Should().Be(first);
    }

    /// <summary>
    /// Tests the property is produced from the sequence.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType=typeof(AsValueTestData))]
    public void GivenLastName_WhenAsValue_ThenPropertyProduced(string first, string last)
    {
        // Given
        var sut = new AsLazyValueTestObject();

        // When
        sut.LastName = last;

        // Then
        sut.FullName.Should().Be(last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType=typeof(AsValueTestData))]
    public void GivenOnChanged_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        var testObject = new AsLazyValueTestObject();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(onChanged: _ => { });

        // When
        testObject.FirstName = first;
        testObject.LastName = last;

        // Then
        sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType=typeof(AsValueTestData))]
    public void GivenOnChangedAndInitialValue_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        var testObject = new AsLazyValueTestObject();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(onChanged: _ => { }, initialValue: () => string.Empty);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;

        // Then
        sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType=typeof(AsValueTestData))]
    public void GivenOnChangedAndOnChangingAndInitialValue_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        var testObject = new AsLazyValueTestObject();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(onChanging: _ => { }, onChanged: _ => { }, initialValue: () => string.Empty);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;

        // Then
        sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType=typeof(AsValueTestData))]
    public void GivenOnChangedAndOnChangingAndSchedulerAndInitialValue_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        var scheduler = new TestScheduler();
        var testObject = new AsLazyValueTestObject();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(onChanged: _ => { }, scheduler, initialValue: () => string.Empty);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;
        scheduler.Start();

        // Then
        sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the value of the value binder.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(AsValueTestData.Data), MemberType=typeof(AsValueTestData))]
    public void GivenAllParameters_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        var scheduler = new TestScheduler();
        var testObject = new AsLazyValueTestObject();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsLazyValue(_ => { }, _ => { }, scheduler, () => string.Empty);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;
        scheduler.Start();

        // Then
        sut.Value.Should().Be(first + last);
    }
}
