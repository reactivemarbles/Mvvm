using global::System.Reactive.Linq;
using global::ReactiveMarbles.Mvvm;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Tests.TestFiles
{
    /// <summary>
    /// This is a test object to prove source generation of value binders.
    /// </summary>
    public partial class TestObject
    {
        [AsValuePropertyOverride(PropertyName = "DifferentPropertyName", InitialValue = "Hello")]
        private IValueBinder<string> _myPropertyName;

        public TestObject()
        {
            // _myPropertyName = this.AsValue(Observable.Return("Hello World"), nameof(_myPropertyName));
            _myPropertyName = Observable.Return("Hello World!").AsValue(this, nameof(_myPropertyName));
        }
    }
}