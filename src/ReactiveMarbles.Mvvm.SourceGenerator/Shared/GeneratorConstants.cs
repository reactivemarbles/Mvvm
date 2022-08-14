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
        public const string AsValueExtensionsSource = @"#pragma warning disable
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[global::System.Diagnostics.DebuggerNonUserCode]
[global::System.Reflection.Obfuscation(Exclude = true)]
internal partial static class AsValueExtensions
{
    public partial static void AsValue<T, TObject>(this global::System.IObservable<T> observable, TObject sourceObject, global::System.Linq.Expressions.Expression<Func<TObject, T>> variableName, [global::System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = null, [global::System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0, [global::System.Runtime.CompilerServices.CallerArgumentExpression(nameof(variableName)] string callerArgumentExpression = null)
        where TObject : global::System.ComponentModel.INotifyPropertyChanged
    {
        return default!;
    }
}";
    }
}
