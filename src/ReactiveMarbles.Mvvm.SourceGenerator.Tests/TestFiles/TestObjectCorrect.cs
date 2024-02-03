using System.ComponentModel;
using System.Reactive.Linq;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Tests.TestFiles;

/// <summary>
/// This is a test object to prove source generation of value binders.
/// </summary>
public partial class TestObjectCorrect
{
    [AsValuePropertyOverride(PropertyName = "DifferentPropertyName", InitialValue = "Hello")]
    private IValueBinder<string> _myPropertyName;

    public TestObjectCorrect()
    {
        // _myPropertyName = this.AsValue(Observable.Return("Hello World"), nameof(_myPropertyName));
        // _myPropertyName = Observable.Return("Hello World!").AsValue(this, nameof(_myPropertyName));
    }
}

/// <summary>
/// This is a test object to prove source generation of value binders.
/// </summary>
public partial class TestObjectCorrect
{
    public string PropertyName => _myPropertyName.Value;
}

internal static partial class AsValueGeneratedExtensions
{
    public static global::ReactiveMarbles.Mvvm.ValueBinder<string> AsValue(
        this TestObjectCorrect sourceObject,
        global::System.IObservable<string> observable,
        string fieldName,
        [global::System.Runtime.CompilerServices.CallerMemberName] string? callerMemberName = null,
        [global::System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0,
        [global::System.Runtime.CompilerServices.CallerArgumentExpression(nameof(fieldName))]
        string? callerArgumentExpression = null)
    {
        return new ValueBinder<string>(observable, _ => { }, initialValue: () => string.Empty);
    }
}