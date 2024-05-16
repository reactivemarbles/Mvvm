// Copyright (c) 2019-2023 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using global::ReactiveMarbles.Mvvm;
using global::System.ComponentModel;

[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[global::System.Diagnostics.DebuggerNonUserCode]
[global::System.Reflection.Obfuscation(Exclude = true)]

internal static partial class AsValueGeneratedExtensions
{
    public static global::ReactiveMarbles.Mvvm.ValueBinder<T> AsValue<T, TObject>(
        this TObject sourceObject,
        global::System.IObservable<T> observable,
        string fieldName,
        [global::System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = null,
        [global::System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0,
        [global::System.Runtime.CompilerServices.CallerArgumentExpression(nameof(fieldName))]
        string callerArgumentExpression = null)
        where TObject : global::System.ComponentModel.INotifyPropertyChanged
    {
        return new ValueBinder<T>(observable, obj => { }, () => default);
    }
}
