using System.Reactive.Linq;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Tests;

public partial class TestObject
{
    public TestObject()
    {
        this.AsValue(Observable.Return("Hello World"), nameof(_myPropertyName));
        // _myPropertyName = Observable.Return("Hello World!").AsValue(this, nameof(_myPropertyName));
    }

    [AsValuePropertyOverride(PropertyName = "DifferentPropertyName", InitialValue="Hello")]
    private ValueBinder<string> _myPropertyName;
}
