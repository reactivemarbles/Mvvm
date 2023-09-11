// Copyright (c) 2019-2023 ReactiveUI Association Incorporated. All rights reserved.
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
    public partial static global::ReactiveMarbles.Mvvm.ValueBinder<T> AsValue<T, TObject>(this TObject sourceObject, global::System.IObservable<T> observable, string fieldName, [global::System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = null, [global::System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0, [global::System.Runtime.CompilerServices.CallerArgumentExpression(nameof(variableName)] string callerArgumentExpression = null)
        where TObject : global::System.ComponentModel.INotifyPropertyChanged
    {
        return default!;
    }
}
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[global::System.Diagnostics.DebuggerNonUserCode]
[global::System.Reflection.Obfuscation(Exclude = true)]
internal class AsValuePropertyOverrideAttribute : global::System.Attribute
{
    public object InitialValue { get; set; }
    public string PropertyName { get; set; }
}
";
    }
}
