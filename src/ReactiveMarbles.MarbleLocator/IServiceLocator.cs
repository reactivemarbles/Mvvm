// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace ReactiveMarbles.MarbleLocator
{
    /// <summary>
    /// IService Locator.
    /// </summary>
    /// <seealso cref="System.IServiceProvider" />
    public interface IServiceLocator : IGetServices, IEditServices
    {
    }
}
