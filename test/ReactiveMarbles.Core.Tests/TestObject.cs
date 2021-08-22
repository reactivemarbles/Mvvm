// Copyright (c) 2021 Reactive Marbles. All rights reserved.
// The Reactive Marbles licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace ReactiveMarbles.Core.Tests
{
    /// <summary>
    /// Test the thing.
    /// </summary>
    public class TestObject : ReactiveObject
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
}
