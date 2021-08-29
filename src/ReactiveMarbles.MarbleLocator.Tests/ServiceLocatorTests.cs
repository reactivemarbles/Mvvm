// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Xunit;

namespace ReactiveMarbles.MarbleLocator.Tests
{
    /// <summary>
    /// Service Locator Tests.
    /// </summary>
    public class ServiceLocatorTests
    {
        /// <summary>
        /// The Service locator has an instance.
        /// </summary>
        [Fact]
        public void ServiceLocatorHasInstance()
        {
            var fixture = ServiceLocator.Current();
            Assert.NotNull(fixture);
        }
    }
}
