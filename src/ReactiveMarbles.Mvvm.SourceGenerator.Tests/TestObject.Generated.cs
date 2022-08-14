using System;
using System.Reactive.Disposables;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Tests;

public partial class TestObject : RxObject
{
    private ValueBinder<string> AsValue(IObservable<string> source, string fieldName)
    {
        switch (fieldName)
        {
            case nameof(_myPropertyName):
                return new ValueBinder<string>(source,
                    _ => RaisePropertyChanging(nameof(DifferentPropertyName)),
                    _ => RaisePropertyChanged(nameof(DifferentPropertyName)),
                    () => string.Empty);
            default:
                throw new InvalidOperationException("You broke it!");
        }
    }

    public string DifferentPropertyName => _myPropertyName.Value;
}
