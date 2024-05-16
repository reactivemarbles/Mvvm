using global::System.Reactive.Linq;
using global::ReactiveMarbles.Mvvm;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Tests.TestFiles
{
    /// <summary>
    /// This is a test object to prove source generation of value binders.
    /// </summary>
    public partial class TestObject
    {
        [AsValuePropertyOverride]
        // TODO: [rlittlesii: February 15, 2024] Create Property with PascalCase name with lambda exposing field.Value;
        private IValueBinder<string> _myPropertyName;

        public TestObject()
        {
            // _myPropertyName = this.AsValue(Observable.Return("Hello World"), nameof(_myPropertyName));
            _myPropertyName = Observable.Return("Hello World!").AsValue(this);
            _myPropertyName = Observable.Return("Hello World!").AsValue(this);
            _myPropertyName = Observable.Return("Hello World!")
                .AsValue(_ => RaisePropertyChanged(nameof(LastKnown)), initialValue: () => new LocationChange(new Position(29.895720, -95.522340), clock.GetCurrentInstant()));
        }

        public string MyPropertyName => _myPropertyName.Value;
    }
}