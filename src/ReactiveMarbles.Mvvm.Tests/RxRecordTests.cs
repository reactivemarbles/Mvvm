// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using ReactiveMarbles.Locator;
using ReactiveMarbles.PropertyChanged;
using ReactiveUI;
using Xunit;

namespace ReactiveMarbles.Mvvm.Tests;

/// <summary>
/// Tests for the <see cref="RxRecord"/>.
/// </summary>
public class RxRecordTests
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RxRecordTests"/> class.
    /// </summary>
    public RxRecordTests() => ServiceLocator.Current().AddCoreRegistrations(() =>
        CoreRegistrationBuilder
            .Create()
            .WithMainThreadScheduler(new TestScheduler())
            .WithTaskPoolScheduler(new TestScheduler())
            .WithExceptionHandler(new DebugExceptionHandler())
            .Build());

    /// <summary>
    /// Test that changing values should always arrive before changed.
    /// </summary>
    [Fact]
    public void ChangingShouldAlwaysArriveBeforeChanged()
    {
        var beforeSet = "Foo";
        var afterSet = "Bar";

        var fixture = new RxRecordTestFixture { IsOnlyOneWord = beforeSet };

        var beforeFired = false;
        fixture.Changing.Subscribe(
            _ =>
            {
                // XXX: The content of these asserts don't actually get
                // propagated back, it only prevents before_fired from
                // being set - we have to enable 1st-chance exceptions
                // to see the real error
                Assert.Equal("IsOnlyOneWord", _.PropertyName);
                Assert.Equal(fixture.IsOnlyOneWord, beforeSet);
                beforeFired = true;
            });

        var afterFired = false;
        fixture.Changed.Subscribe(
            _ =>
            {
                Assert.Equal("IsOnlyOneWord", _.PropertyName);
                Assert.Equal(fixture.IsOnlyOneWord, afterSet);
                afterFired = true;
            });

        fixture.IsOnlyOneWord = afterSet;

        beforeFired.Should().BeTrue();
        afterFired.Should().BeTrue();
    }

    /// <summary>
    /// Test that deferring the notifications dont show up until un-deferred.
    /// </summary>
    [Fact]
    public void DeferredNotificationsDontShowUpUntilUndeferred()
    {
        var fixture = new RxRecordTestFixture();
        fixture.Changing.ToObservableChangeSet(ImmediateScheduler.Instance).Bind(out var changing).Subscribe();
        fixture.Changed.ToObservableChangeSet(ImmediateScheduler.Instance).Bind(out var changed).Subscribe();
        var propertyChangingEvents = new List<PropertyChangingEventArgs>();
        fixture.PropertyChanging += (_, args) => propertyChangingEvents.Add(args);
        var propertyChangedEvents = new List<PropertyChangedEventArgs>();
        fixture.PropertyChanged += (_, args) => propertyChangedEvents.Add(args);

        AssertCount(0, changing, changed, propertyChangingEvents, propertyChangedEvents);
        fixture.NullableInt = 4;
        AssertCount(1, changing, changed, propertyChangingEvents, propertyChangedEvents);

        var stopDelaying = fixture.DelayChangeNotifications();

        fixture.NullableInt = 5;
        AssertCount(1, changing, changed, propertyChangingEvents, propertyChangedEvents);

        fixture.IsNotNullString = "Bar";
        AssertCount(1, changing, changed, propertyChangingEvents, propertyChangedEvents);

        fixture.NullableInt = 6;
        AssertCount(1, changing, changed, propertyChangingEvents, propertyChangedEvents);

        fixture.IsNotNullString = "Baz";
        AssertCount(1, changing, changed, propertyChangingEvents, propertyChangedEvents);

        var stopDelayingMore = fixture.DelayChangeNotifications();

        fixture.IsNotNullString = "Bamf";
        AssertCount(1, changing, changed, propertyChangingEvents, propertyChangedEvents);

        stopDelaying.Dispose();

        fixture.IsNotNullString = "Blargh";
        AssertCount(1, changing, changed, propertyChangingEvents, propertyChangedEvents);

        // NB: Because we debounce queued up notifications, we should only
        // see a notification from the latest NullableInt and the latest
        // IsNotNullableString
        stopDelayingMore.Dispose();

        AssertCount(3, changing, changed, propertyChangingEvents, propertyChangedEvents);

        var expectedEventProperties = new[] { "NullableInt", "NullableInt", "IsNotNullString" };
        Assert.Equal(expectedEventProperties, changing.Select(e => e.PropertyName));
        Assert.Equal(expectedEventProperties, changed.Select(e => e.PropertyName));
        Assert.Equal(expectedEventProperties, propertyChangingEvents.Select(e => e.PropertyName));
        Assert.Equal(expectedEventProperties, propertyChangedEvents.Select(e => e.PropertyName));
    }

    /// <summary>
    /// Test that exceptions thrown in subscribers should marshal to thrown exceptions.
    /// </summary>
    [Fact]
    public void ExceptionsThrownInSubscribersShouldMarshalToThrownExceptions()
    {
        var fixture = new RxRecordTestFixture { IsOnlyOneWord = "Foo" };

        fixture.Changed.Subscribe(_ => throw new Exception("Terminate!"));
        fixture.ThrownExceptions.ToObservableChangeSet(ImmediateScheduler.Instance).Bind(out var exceptionList)
            .Subscribe();

        fixture.IsOnlyOneWord = "Bar";

        exceptionList.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests that ObservableForProperty using expression.
    /// </summary>
    [Fact]
    public void ObservableForPropertyUsingExpression()
    {
        var fixture = new RxRecordTestFixture { IsNotNullString = "Foo", IsOnlyOneWord = "Baz" };
        var output = new List<IObservedChange<RxRecordTestFixture, string?>>();
        fixture.ObservableForProperty(x => x.IsNotNullString)
            .Subscribe(x => output.Add(x));

        fixture.IsNotNullString = "Bar";
        fixture.IsNotNullString = "Baz";
        fixture.IsNotNullString = "Baz";

        fixture.IsOnlyOneWord = "Bamf";

        output.Should().HaveCount(2);
        output[0].Sender.Should().Be(fixture);
        output[0].GetPropertyName().Should().Be("IsNotNullString");
        output[0].Value.Should().Be("Bar");
        output[1].Sender.Should().Be(fixture);
        output[1].GetPropertyName().Should().Be("IsNotNullString");
        output[1].Value.Should().Be("Baz");
    }

    /// <summary>
    /// Test raises and set using expression.
    /// </summary>
    [Fact]
    public void RaiseAndSetUsingExpression()
    {
        var fixture = new RxRecordTestFixture { IsNotNullString = "Foo", IsOnlyOneWord = "Baz" };
        var output = new List<string>();
        fixture.Changed
            .Select(x => x.PropertyName)
            .Subscribe(x => output.Add(x));

        fixture.UsesExprRaiseSet = "Foo";
        fixture.UsesExprRaiseSet = "Foo"; // This one shouldn't raise a change notification

        fixture.UsesExprRaiseSet.Should().Be("Foo");
        output.Should().HaveCount(1);
        output.Should().ContainSingle(x => x == "UsesExprRaiseSet");
    }

    /// <summary>
    /// Test that RxRecord shouldn't serialize anything extra.
    /// </summary>
    [Fact(Skip = "JSONHelper")]
    public void RxRecordShouldntSerializeAnythingExtra()
    {
        // var fixture = new TestFixture
        // {
        //     IsNotNullString = "Foo",
        //     IsOnlyOneWord = "Baz"
        // };
        // var json = JSONHelper.Serialize(fixture);
        //
        // if (json is null)
        // {
        //     throw new InvalidOperationException("JSON string should not be null");
        // }
        //
        // // Should look something like:
        // // {"IsNotNullString":"Foo","IsOnlyOneWord":"Baz","NullableInt":null,"PocoProperty":null,"StackOverflowTrigger":null,"TestCollection":[],"UsesExprRaiseSet":null}
        // Assert.True(json.Count(x => x == ',') == 6);
        // Assert.True(json.Count(x => x == ':') == 7);
        // Assert.True(json.Count(x => x == '"') == 18);
    }

    /// <summary>
    /// Performs a RxRecord smoke test.
    /// </summary>
    [Fact]
    public void RxRecordSmokeTest()
    {
        var outputChanging = new List<string>();
        var output = new List<string>();
        var fixture = new RxRecordTestFixture();

        fixture.Changing
            .Select(x => x.PropertyName)
            .Subscribe(x => outputChanging.Add(x));

        fixture.Changed
            .Select(x => x.PropertyName)
            .Subscribe(x => output.Add(x));

        fixture.IsNotNullString = "Foo Bar Baz";
        fixture.IsOnlyOneWord = "Foo";
        fixture.IsOnlyOneWord = "Bar";
        fixture.IsNotNullString = null; // Sorry.
        fixture.IsNotNullString = null;

        var results = new[] { "IsNotNullString", "IsOnlyOneWord", "IsOnlyOneWord", "IsNotNullString" };

        output.Should().BeEquivalentTo(outputChanging);
        results.Length.Should().Be(output.Count);
        results.Should().BeEquivalentTo(output);
    }

    /// <summary>
    /// Tests to make sure that RxRecord doesn't rethrow exceptions.
    /// </summary>
    [Fact]
    public void RxRecordShouldRethrowException()
    {
        var fixture = new RxRecordTestFixture();
        var observable = fixture.WhenChanged(x => x.IsOnlyOneWord).Skip(1);
        observable.Subscribe(_ => throw new Exception("This is a test."));

        var result = Record.Exception(() => fixture.IsOnlyOneWord = "Two Words");

        result.Should()
            .BeOfType<Exception>()
            .Which
            .Message
            .Should()
            .Be("This is a test.");
    }

    /// <summary>
    /// Tests that change notifications are not delayed.
    /// </summary>
    [Fact]
    public void ChangeNotificationsAreNotDelayed() =>

        // Given, When, Then
        new RxRecordTestFixture()
            .AreChangeNotificationsDelayed()
            .Should()
            .BeFalse();

    /// <summary>
    /// Tests that change notifications are delayed.
    /// </summary>
    [Fact]
    public void ChangeNotificationsAreDelayed()
    {
        // Given
        var fixture = new RxRecordTestFixture();

        // When
        using var disposable = fixture.DelayChangeNotifications();

        // Then
        fixture
            .AreChangeNotificationsDelayed()
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Tests that change notifications are enabled.
    /// </summary>
    [Fact]
    public void ChangeNotificationsAreEnabled() =>

        // Given, When, Then
        new RxRecordTestFixture()
            .AreChangeNotificationsEnabled()
            .Should()
            .BeTrue();

    /// <summary>
    /// Tests that change notifications are suppressed.
    /// </summary>
    [Fact]
    public void ChangeNotificationsAreSuppressed()
    {
        // Given
        var fixture = new RxRecordTestFixture();

        // When
        using var disposable = fixture.SuppressChangeNotifications();

        // Then
        fixture
            .AreChangeNotificationsEnabled()
            .Should()
            .BeFalse();
    }

    private static void AssertCount(int expected, params ICollection[] collections)
    {
        foreach (var collection in collections)
        {
            Assert.Equal(expected, collection.Count);
        }
    }
}
