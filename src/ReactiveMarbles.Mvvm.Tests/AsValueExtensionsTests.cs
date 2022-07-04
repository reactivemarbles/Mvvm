// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using ReactiveMarbles.PropertyChanged;
using Xunit;

namespace ReactiveMarbles.Mvvm.Tests;

/// <summary>
/// Tests for the <see cref="AsValueExtensions"/>.
/// </summary>
public class AsValueExtensionsTests
{
    /// <summary>
    /// Gets test data.
    /// </summary>
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { "Leeroy", "Jenkins" },
            new object[] { "James", "Kirk" },
            new object[] { "Major", "Payne" },
        };

    /// <summary>
    /// Tests the default value.
    /// </summary>
    [Fact]
    public void GivenNoChanges_WhenAsValue_ThenFullNameIsEmpty()
    {
        // Given, When
        var sut = new AsValueTestObject();

        // Then
        sut.FullName.Should().BeNullOrEmpty();
    }

    /// <summary>
    /// Tests the property is produced from the sequence.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(Data))]
    public void GivenSequence_WhenAsValue_ThenPropertyProduced(string first, string last)
    {
        // Given
        var sut = new AsValueTestObject();

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
    [MemberData(nameof(Data))]
    public void GivenFirstName_WhenAsValue_ThenPropertyProduced(string first, string last)
    {
        // Given
        var sut = new AsValueTestObject();

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
    [MemberData(nameof(Data))]
    public void GivenLastName_WhenAsValue_ThenPropertyProduced(string first, string last)
    {
        // Given
        var sut = new AsValueTestObject();

        // When
        sut.LastName = last;

        // Then
        sut.FullName.Should().Be(last);
    }

    /// <summary>
    /// Tests the default value.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(Data))]
    public void GivenName_WhenAsValueWithOnChanged_ThenValueCorrect(string first, string last)
    {
        // Given
        var testObject = new AsValueTestObject();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsValue(onChanged: tuple => { });

        // When
        testObject.FirstName = first;
        testObject.LastName = last;

        // Then
        sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the default value.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(Data))]
    public void GivenOnChangedAndOnChangingAndInitialValue_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        var testObject = new AsValueTestObject();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsValue(onChanging: _ => { }, onChanged: _ => { }, initialValue: () => string.Empty);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;

        // Then
        sut.Value.Should().Be(first + last);
    }

    /// <summary>
    /// Tests the default value.
    /// </summary>
    /// <param name="first">The first name.</param>
    /// <param name="last">The last name.</param>
    [Theory]
    [MemberData(nameof(Data))]
    public void GivenAllParameters_WhenAsValue_ThenValueCorrect(string first, string last)
    {
        // Given
        var scheduler = new TestScheduler();
        var testObject = new AsValueTestObject();
        var sut =
            testObject
                .WhenChanged(x => x.FirstName, x => x.LastName, (firstName, lastName) => firstName + lastName)
                .AsValue(_ => { }, _ => { }, scheduler, () => string.Empty);

        // When
        testObject.FirstName = first;
        testObject.LastName = last;
        scheduler.Start();

        // Then
        sut.Value.Should().Be(first + last);
    }
}
