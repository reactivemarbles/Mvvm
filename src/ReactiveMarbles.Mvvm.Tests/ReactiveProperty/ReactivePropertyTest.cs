// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using ReactiveMarbles.Mvvm.Testing;
using ReactiveMarbles.Mvvm.Tests.ReactiveProperty.Mocks;
using Xunit;

namespace ReactiveMarbles.Mvvm.Tests.ReactiveProperty
{
    /// <summary>
    /// ReactivePropertyTest.
    /// </summary>
    /// <seealso cref="Microsoft.Reactive.Testing.ReactiveTest" />
    public class ReactivePropertyTest : ReactiveTest
    {
        /// <summary>
        /// Defaults the value is raised on subscribe.
        /// </summary>
        [Fact]
        public void DefaultValueIsRaisedOnSubscribe()
        {
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>();
            rp.Value.Should().BeNull();
            rp.Subscribe(Assert.Null);
        }

        /// <summary>
        /// Initials the value.
        /// </summary>
        [Fact]
        public void InitialValue()
        {
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>("ReactiveUI");
            Assert.Equal(rp.Value, "ReactiveUI");
            rp.Subscribe(x => Assert.Equal(x, "ReactiveUI"));
        }

        /// <summary>
        /// Initials the value skip current.
        /// </summary>
        [Fact]
        public void InitialValueSkipCurrent()
        {
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>("ReactiveUI", true, false);
            Assert.Equal(rp.Value, "ReactiveUI");

            // current value should be skipped
            rp.Subscribe(x => Assert.Equal(x, "ReactiveUI 2"));
            rp.Value = "ReactiveUI 2";
            Assert.Equal(rp.Value, "ReactiveUI 2");
        }

        /// <summary>
        /// Sets the value raises events.
        /// </summary>
        [Fact]
        public void SetValueRaisesEvents()
        {
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>();
            rp.Value.Should().BeNull();
            rp.Value = "ReactiveUI";
            Assert.Equal(rp.Value, "ReactiveUI");
            rp.Subscribe(x => Assert.Equal(x, "ReactiveUI"));
        }

        /// <summary>
        /// Validations the length is correctly handled.
        /// </summary>
        [Fact]
        public void ValidationLengthIsCorrectlyHandled()
        {
            var target = new ReactivePropertyVM();
            IEnumerable? error = default;
            target.LengthLessThanFiveProperty
                .ObserveErrorChanged
                .Subscribe(x => error = x);

            target.LengthLessThanFiveProperty.HasErrors.Should().BeTrue();
            Assert.Equal(error?.OfType<string>().First(), "required");

            target.LengthLessThanFiveProperty.Value = "a";
            target.LengthLessThanFiveProperty.HasErrors.Should().BeFalse();
            error.Should().BeNull();

            target.LengthLessThanFiveProperty.Value = "aaaaaa";
            target.LengthLessThanFiveProperty.HasErrors.Should().BeTrue();
            error.Should().NotBeNull();
            Assert.Equal(error?.OfType<string>().First(), "5over");

            target.LengthLessThanFiveProperty.Value = null;
            target.LengthLessThanFiveProperty.HasErrors.Should().BeTrue();
            Assert.Equal(error?.OfType<string>().First(), "required");
        }

        /// <summary>
        /// Validations the is required is correctly handled.
        /// </summary>
        [Fact]
        public void ValidationIsRequiredIsCorrectlyHandled()
        {
            var target = new ReactivePropertyVM();
            var errors = new List<IEnumerable?>();
            target.IsRequiredProperty
                .ObserveErrorChanged
                .Where(x => x != null)
                .Subscribe(errors.Add);

            errors.Count.Should().Be(1);
            errors[0]?.Cast<string>().Should().Equal("error!");
            target.IsRequiredProperty.HasErrors.Should().BeTrue();

            target.IsRequiredProperty.Value = "a";
            errors.Count.Should().Be(1);
            target.IsRequiredProperty.HasErrors.Should().BeFalse();

            target.IsRequiredProperty.Value = null;
            errors.Count.Should().Be(2);
            errors[1]?.Cast<string>().Should().Equal("error!");
            target.IsRequiredProperty.HasErrors.Should().BeTrue();
        }

        /// <summary>
        /// Validations the task test.
        /// </summary>
        [Fact]
        public void ValidationTaskTest()
        {
            var target = new ReactivePropertyVM();
            var errors = new List<IEnumerable?>();
            target.TaskValidationTestProperty
                .ObserveErrorChanged
                .Where(x => x != null)
                .Subscribe(errors.Add);
            errors.Count.Should().Be(1);
            errors[0]?.OfType<string>().Should().Equal("required");

            target.TaskValidationTestProperty.Value = "a";
            target.TaskValidationTestProperty.HasErrors.Should().BeFalse();
            errors.Count.Should().Be(1);

            target.TaskValidationTestProperty.Value = null;
            target.TaskValidationTestProperty.HasErrors.Should().BeTrue();
            errors.Count.Should().Be(2);
        }

        /// <summary>
        /// Validations the with custom error message.
        /// </summary>
        [Fact]
        public void ValidationWithCustomErrorMessage()
        {
            var target = new ReactivePropertyVM();
            target.CustomValidationErrorMessageProperty.Value = string.Empty;
            var errorMessage = target?
                .CustomValidationErrorMessageProperty?
                .GetErrors(nameof(ReactivePropertyVM.CustomValidationErrorMessageProperty))!
                .Cast<string>()
                .First();

            Assert.Equal(errorMessage, "Custom validation error message for CustomValidationErrorMessageProperty");
        }

        /// <summary>
        /// Validations the display name of the with custom error message with.
        /// </summary>
        [Fact]
        public void ValidationWithCustomErrorMessageWithDisplayName()
        {
            var target = new ReactivePropertyVM();
            target.CustomValidationErrorMessageWithDisplayNameProperty.Value = string.Empty;
            var errorMessage = target
                .CustomValidationErrorMessageWithDisplayNameProperty?
                .GetErrors(nameof(ReactivePropertyVM.CustomValidationErrorMessageWithDisplayNameProperty))!
                .Cast<string>()
                .First();

            Assert.Equal(errorMessage, "Custom validation error message for CustomName");
        }

        /// <summary>
        /// Validations the with custom error message with resource.
        /// </summary>
        [Fact]
        public void ValidationWithCustomErrorMessageWithResource()
        {
            var target = new ReactivePropertyVM();
            target.CustomValidationErrorMessageWithResourceProperty.Value = string.Empty;
            var errorMessage = target
                .CustomValidationErrorMessageWithResourceProperty?
                .GetErrors(nameof(ReactivePropertyVM.CustomValidationErrorMessageWithResourceProperty))!
                .Cast<string>()
                .First();

            Assert.Equal(errorMessage, "Oops!? FromResource is required.");
        }

        /// <summary>
        /// Validations the with asynchronous success case.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ValidationWithAsyncSuccessCase()
        {
            var tcs = new TaskCompletionSource<string?>();
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>().AddValidationError(_ => tcs.Task);

            IEnumerable? error = null;
            rp.ObserveErrorChanged.Subscribe(x => error = x);

            rp.HasErrors.Should().BeFalse();
            error.Should().BeNull();

            rp.Value = "dummy";
            tcs.SetResult(null);
            await Task.Yield();

            rp.HasErrors.Should().BeFalse();
            error.Should().BeNull();
        }

        /// <summary>
        /// Validations the with asynchronous failed case.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ValidationWithAsyncFailedCase()
        {
            var tcs = new TaskCompletionSource<string?>();
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>().AddValidationError(_ => tcs.Task);

            IEnumerable? error = null;
            rp.ObserveErrorChanged.Subscribe(x => error = x);

            rp.HasErrors.Should().BeFalse();
            error.Should().BeNull();

            var errorMessage = "error occured!!";
            rp.Value = "dummy";  //--- push value
            tcs.SetResult(errorMessage);    //--- validation error!
            await Task.Delay(10);

            rp.HasErrors.Should().BeTrue();
            error.Should().NotBeNull();
            error?.Cast<string>().Should().Equal(errorMessage);
            rp.GetErrors("Value")?.Cast<string>().Should().Equal(errorMessage);
        }

        /// <summary>
        /// Validations the with asynchronous throttle test.
        /// </summary>
        [Fact]
        public void ValidationWithAsyncThrottleTest()
        {
            var scheduler = new TestScheduler();
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>()
                            .AddValidationError(xs => xs
                                .Throttle(TimeSpan.FromSeconds(1), scheduler)
                                .Select(x => string.IsNullOrEmpty(x) ? "required" : null));

            IEnumerable? error = null;
            rp.ObserveErrorChanged.Subscribe(x => error = x);

            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(0).Ticks);
            rp.Value = string.Empty;
            rp.HasErrors.Should().BeFalse();
            error.Should().BeNull();

            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(300).Ticks);
            rp.Value = "a";
            rp.HasErrors.Should().BeFalse();
            error.Should().BeNull();

            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(700).Ticks);
            rp.Value = "b";
            rp.HasErrors.Should().BeFalse();
            error.Should().BeNull();

            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(1100).Ticks);
            rp.Value = string.Empty;
            rp.HasErrors.Should().BeFalse();
            error.Should().BeNull();

            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(2500).Ticks);
            rp.HasErrors.Should().BeTrue();
            error.Should().NotBeNull();
            error?.Cast<string>().Should().Equal("required");
        }

        /// <summary>
        /// Validations the error changed test.
        /// </summary>
        [Fact]
        public void ValidationErrorChangedTest()
        {
            var errors = new List<IEnumerable?>();
            var rprop = new ReactiveMarbles.Mvvm.ReactiveProperty<string>()
                .AddValidationError(x => string.IsNullOrWhiteSpace(x) ? "error" : null);

            // old version behavior
            rprop.ObserveErrorChanged.Skip(1).Subscribe(errors.Add);

            errors.Count.Should().Be(0);

            rprop.Value = "OK";
            errors.Count.Should().Be(1);
            errors.Last().Should().BeNull();

            rprop.Value = null;
            errors.Count.Should().Be(2);
            errors.Last()?.OfType<string>().Should().Equal("error");
        }

        /// <summary>
        /// Validations the ignore initial error and refresh.
        /// </summary>
        [Fact]
        public void ValidationIgnoreInitialErrorAndRefresh()
        {
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>()
                .AddValidationError(x => string.IsNullOrEmpty(x) ? "error" : null, true);

            rp.HasErrors.Should().BeFalse();
            rp.Refresh();
            rp.HasErrors.Should().BeTrue();
        }

        /// <summary>
        /// Ignores the initial error and check validation.
        /// </summary>
        [Fact]
        public void IgnoreInitialErrorAndCheckValidation()
        {
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>()
                .AddValidationError(x => string.IsNullOrEmpty(x) ? "error" : null, true);

            rp.HasErrors.Should().BeFalse();
            rp.CheckValidation();
            rp.HasErrors.Should().BeTrue();
        }

        /// <summary>
        /// Ignores the initialize error and update value.
        /// </summary>
        [Fact]
        public void IgnoreInitErrorAndUpdateValue()
        {
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>()
                .AddValidationError(x => string.IsNullOrEmpty(x) ? "error" : null, true);

            rp.HasErrors.Should().BeFalse();
            rp.Value = string.Empty;
            rp.HasErrors.Should().BeTrue();
        }

        /// <summary>
        /// Observes the errors.
        /// </summary>
        [Fact]
        public void ObserveErrors()
        {
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>()
                .AddValidationError(x => x == null ? "Error" : null);

            var results = new List<IEnumerable?>();
            rp.ObserveErrorChanged.Subscribe(results.Add);
            rp.Value = "OK";

            results.Count.Should().Be(2);
            results[0]?.OfType<string>().Should().Equal("Error");
            results[1].Should().BeNull();
        }

        /// <summary>
        /// Observes the has error.
        /// </summary>
        [Fact]
        public void ObserveHasError()
        {
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<string>()
                .AddValidationError(x => x == null ? "Error" : null);

            var results = new List<bool>();
            rp.ObserveHasErrors.Subscribe(x => results.Add(x));
            rp.Value = "OK";

            results.Count.Should().Be(2);
            results[0].Should().BeTrue();
            results[1].Should().BeFalse();
        }

        /// <summary>
        /// Checks the validation.
        /// </summary>
        [Fact]
        public void CheckValidation()
        {
            var minValue = 0;
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<int>(0)
                .AddValidationError(x => x < minValue ? "Error" : null);
            rp.GetErrors("Value").Should().BeNull();

            minValue = 1;
            rp.GetErrors("Value").Should().BeNull();

            rp.CheckValidation();
            rp.GetErrors("Value")?.OfType<string>().Should().Equal("Error");
        }

        /// <summary>
        /// Values the updates multiple times with different values.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ValueUpdatesMultipleTimesWithDifferentValues()
        {
            using var testSequencer = new TestSequencer();
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<int>(0);
            var collector = new List<int>();
            rp.Subscribe(async x =>
            {
                collector.Add(x);
                await testSequencer.AdvancePhaseAsync();
            });

            rp.Value.Should().Be(0);
            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0);
            rp.Value = 1;
            rp.Value.Should().Be(1);
            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0, 1);
            rp.Value = 2;
            rp.Value.Should().Be(2);
            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0, 1, 2);
            rp.Value = 3;
            rp.Value.Should().Be(3);
            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0, 1, 2, 3);
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Refresh()
        {
            using var testSequencer = new TestSequencer();
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<int>(0);
            var collector = new List<int>();
            rp.Subscribe(async x =>
            {
                collector.Add(x);
                await testSequencer.AdvancePhaseAsync();
            });

            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0);
            rp.Refresh();
            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0, 0);
        }

        /// <summary>
        /// Values the updates multiple times with same values.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ValueUpdatesMultipleTimesWithSameValues()
        {
            using var testSequencer = new TestSequencer();
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<int>(0, false, true);
            var collector = new List<int>();
            rp.Subscribe(async x =>
            {
                collector.Add(x);
                await testSequencer.AdvancePhaseAsync();
            });

            rp.Value.Should().Be(0);
            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0);
            rp.Value = 0;
            rp.Value.Should().Be(0);
            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0, 0);
            rp.Value = 0;
            rp.Value.Should().Be(0);
            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0, 0, 0);
            rp.Value = 0;
            rp.Value.Should().Be(0);
            await testSequencer.AdvancePhaseAsync();
            collector.Should().Equal(0, 0, 0, 0);
        }

        /// <summary>
        /// Multiples the subscribers get current value.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MultipleSubscribersGetCurrentValue()
        {
            using var testSequencer1 = new TestSequencer();
            using var testSequencer2 = new TestSequencer();
            var rp = new ReactiveMarbles.Mvvm.ReactiveProperty<int>(0);
            var collector1 = new List<int>();
            var collector2 = new List<int>();
            rp.Subscribe(async x =>
            {
                collector1.Add(x);
                await testSequencer1.AdvancePhaseAsync();
            });

            rp.Value.Should().Be(0);
            await testSequencer1.AdvancePhaseAsync();
            collector1.Should().Equal(0);
            rp.Value = 1;
            rp.Value.Should().Be(1);
            await testSequencer1.AdvancePhaseAsync();
            collector1.Should().Equal(0, 1);
            rp.Value = 2;
            rp.Value.Should().Be(2);
            await testSequencer1.AdvancePhaseAsync();
            collector1.Should().Equal(0, 1, 2);

            // second subscriber
            rp.Subscribe(async x =>
            {
                collector2.Add(x);
                await testSequencer2.AdvancePhaseAsync();
            });
            rp.Value.Should().Be(2);
            await testSequencer2.AdvancePhaseAsync();
            collector2.Should().Equal(2);

            rp.Value = 3;
            rp.Value.Should().Be(3);
            await testSequencer1.AdvancePhaseAsync();
            collector1.Should().Equal(0, 1, 2, 3);
            await testSequencer2.AdvancePhaseAsync();
            collector2.Should().Equal(2, 3);
        }
    }
}
