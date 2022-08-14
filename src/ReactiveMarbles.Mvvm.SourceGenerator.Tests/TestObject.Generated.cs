using System;
using System.Reactive.Disposables;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Tests;

public partial class TestObject : RxObject
{
    private IDisposable AsValue(IObservable<string> source, string fieldName)
    {
        switch (fieldName)
        {
            case nameof(_myPropertyName):
                _myPropertyName = new ValueBinder<string>(source,
                    _ => RaisePropertyChanging(nameof(DifferentPropertyName)),
                    _ => RaisePropertyChanged(nameof(DifferentPropertyName)),
                    () => string.Empty);
                break;
        }

        return Disposable.Empty;
    }

    public string DifferentPropertyName => _myPropertyName.Value;
}
