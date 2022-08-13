// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace ReactiveMarbles.Mvvm.SourceGenerator.Shared
{
    /// <summary>
    /// Constants for source generation.
    /// </summary>
    internal static class GeneratorConstants
    {
        public const string AsValueExtensionsSource = @"public partial static class AsValueExtensions
{
    public static global::ReactiveMarbles.Mvvm.ValueBinder<T> AsValue<T>(this global::System.IObservable<T> observable)
    { 
    }
}";
    }
}
