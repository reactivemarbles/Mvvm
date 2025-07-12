// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
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
    public RxRecordTests()
    {
        _ = ServiceLocator.Current().AddCoreRegistrations(() =>
        CoreRegistrationBuilder
            .Create()
            .WithMainThreadScheduler(new TestScheduler())
            .WithTaskPoolScheduler(new TestScheduler())
            .WithExceptionHandler(new DebugExceptionHandler())
            .Build());
    }

    /// <summary>
    /// Test that changing values should always arrive before changed.
    /// </summary>
    [Fact]
    public void ChangingShouldAlwaysArriveBeforeChanged()
    {
        var beforeSet = "Foo";
        var afterSet = "Bar";

        RxRecordTestFixture? fixture = new() { IsOnlyOneWord = beforeSet };

        var beforeFired = false;
        _ = fixture.Changing.Subscribe(
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
        _ = fixture.Changed.Subscribe(
            _ =>
            {
                Assert.Equal("IsOnlyOneWord", _.PropertyName);
                Assert.Equal(fixture.IsOnlyOneWord, afterSet);
                afterFired = true;
            });

        fixture.IsOnlyOneWord = afterSet;

        _ = beforeFired.Should().BeTrue();
        _ = afterFired.Should().BeTrue();
    }

    /// <summary>
    /// Test that deferring the notifications dont show up until un-deferred.
    /// </summary>
    [Fact]
    public void DeferredNotificationsDontShowUpUntilUndeferred()
    {
        RxRecordTestFixture? fixture = new();
        _ = fixture.Changing.ToObservableChangeSet(ImmediateScheduler.Instance).Bind(out var changing).Subscribe();
        _ = fixture.Changed.ToObservableChangeSet(ImmediateScheduler.Instance).Bind(out var changed).Subscribe();
        List<PropertyChangingEventArgs>? propertyChangingEvents = [];
        fixture.PropertyChanging += (_, args) => propertyChangingEvents.Add(args);
        List<PropertyChangedEventArgs>? propertyChangedEvents = [];
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
        RxRecordTestFixture? fixture = new() { IsOnlyOneWord = "Foo" };

        _ = fixture.Changed.Subscribe(_ => throw new Exception("Terminate!"));
        _ = fixture.ThrownExceptions.ToObservableChangeSet(ImmediateScheduler.Instance).Bind(out var exceptionList)
            .Subscribe();

        fixture.IsOnlyOneWord = "Bar";

        _ = exceptionList.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests that ObservableForProperty using expression.
    /// </summary>
    [Fact(Skip = "Using ReactiveUI ObservableForProperty for test, results in invalid test as end user wont be using ReactiveUI")]
    public void ObservableForPropertyUsingExpression()
    {
        RxRecordTestFixture? fixture = new() { IsNotNullString = "Foo", IsOnlyOneWord = "Baz" };
        ////List<IObservedChange<RxRecordTestFixture, string?>>? output = new();
        ////_ = fixture.ObservableForProperty(x => x.IsNotNullString)
        ////    .Subscribe(output.Add);

        fixture.IsNotNullString = "Bar";
        fixture.IsNotNullString = "Baz";
        fixture.IsNotNullString = "Baz";

        fixture.IsOnlyOneWord = "Bamf";

        ////_ = output.Should().HaveCount(2);
        ////_ = output[0].Sender.Should().Be(fixture);
        ////_ = output[0].GetPropertyName().Should().Be("IsNotNullString");
        ////_ = output[0].Value.Should().Be("Bar");
        ////_ = output[1].Sender.Should().Be(fixture);
        ////_ = output[1].GetPropertyName().Should().Be("IsNotNullString");
        ////_ = output[1].Value.Should().Be("Baz");
    }

    /// <summary>
    /// Test raises and set using expression.
    /// </summary>
    [Fact]
    public void RaiseAndSetUsingExpression()
    {
        RxRecordTestFixture? fixture = new() { IsNotNullString = "Foo", IsOnlyOneWord = "Baz" };
        List<string?>? output = [];
        _ = fixture.Changed
            .Select(x => x.PropertyName)
            .Subscribe(output.Add);

        fixture.UsesExprRaiseSet = "Foo";
        fixture.UsesExprRaiseSet = "Foo"; // This one shouldn't raise a change notification

        _ = fixture.UsesExprRaiseSet.Should().Be("Foo");
        _ = output.Should().HaveCount(1);
        _ = output.Should().ContainSingle(x => x == "UsesExprRaiseSet");
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
        List<string?>? outputChanging = [];
        List<string?>? output = [];
        RxRecordTestFixture? fixture = new();

        _ = fixture.Changing
            .Select(x => x.PropertyName)
            .Subscribe(outputChanging.Add);

        _ = fixture.Changed
            .Select(x => x.PropertyName)
            .Subscribe(output.Add);

        fixture.IsNotNullString = "Foo Bar Baz";
        fixture.IsOnlyOneWord = "Foo";
        fixture.IsOnlyOneWord = "Bar";
        fixture.IsNotNullString = null; // Sorry.
        fixture.IsNotNullString = null;

        var results = new[] { "IsNotNullString", "IsOnlyOneWord", "IsOnlyOneWord", "IsNotNullString" };

        _ = output.Should().BeEquivalentTo(outputChanging);
        _ = results.Length.Should().Be(output.Count);
        _ = results.Should().BeEquivalentTo(output);
    }

    /// <summary>
    /// Tests to make sure that RxRecord doesn't rethrow exceptions.
    /// </summary>
    [Fact]
    public void RxRecordShouldRethrowException()
    {
        RxRecordTestFixture? fixture = new();
        var observable = fixture.WhenChanged(x => x.IsOnlyOneWord).Skip(1);
        _ = observable.Subscribe(_ => throw new Exception("This is a test."));

        var result = Record.Exception(() => fixture.IsOnlyOneWord = "Two Words");

        _ = result.Should()
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
    public void ChangeNotificationsAreNotDelayed()
    {
        // Given, When, Then
        _ = new RxRecordTestFixture()
            .AreChangeNotificationsDelayed()
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Tests that change notifications are delayed.
    /// </summary>
    [Fact]
    public void ChangeNotificationsAreDelayed()
    {
        // Given
        RxRecordTestFixture? fixture = new();

        // When
        using var disposable = fixture.DelayChangeNotifications();

        // Then
        _ = fixture
            .AreChangeNotificationsDelayed()
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Tests that change notifications are enabled.
    /// </summary>
    [Fact]
    public void ChangeNotificationsAreEnabled()
    {
        // Given, When, Then
        _ = new RxRecordTestFixture()
            .AreChangeNotificationsEnabled()
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Tests that change notifications are suppressed.
    /// </summary>
    [Fact]
    public void ChangeNotificationsAreSuppressed()
    {
        // Given
        RxRecordTestFixture? fixture = new();

        // When
        using var disposable = fixture.SuppressChangeNotifications();

        // Then
        _ = fixture
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
