# Get Started

## Registering Framework Concerns

`ICoreRegistration` gives the framework an understanding of the following concerns for ReactiveMarbles internals.  We provide a simple builder and extension method to register it against the `ServiceLocator`.

```csharp
ServiceLocator
   .Current()
   .AddCoreRegistrations(() =>
        CoreRegistrationBuilder
           .Create()
           .WithMainThreadScheduler(Scheduler.Default)
           .WithTaskPoolScheduler(TaskPoolScheduler.Default)
           .WithExceptionHandler(new DebugExceptionHandler())
           .Build());
```

## AsValue

`AsValue` allows you to bind an `IObservable<T>` to a property that produces a property changed event.

```csharp
_valueChange =
    this.WhenChanged(x => x.Property)
        .Select(x => x + "Changed")
        .AsValue(onChanged: x => RaisePropertyChanged(nameof(ValueChange)));
```
