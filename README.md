![License](https://img.shields.io/github/license/ReactiveMarbles/Mvvm.svg) [![Build](https://github.com/reactivemarbles/Mvvm/actions/workflows/ci-build.yml/badge.svg)](https://github.com/reactivemarbles/Mvvm/actions/workflows/ci-build.yml)


# Reactive Marbles Mvvm
A light weight mvvm package for Reactive Marbles to introduce framework abstractions

## NuGet packages

| Name                          | Platform          | NuGet                            |
| ----------------------------- | ----------------- | -------------------------------- |
| [ReactiveMarbles.Mvvm][Core]       | Core - Libary     | [![CoreBadge]][Core]             |

[Core]: https://www.nuget.org/packages/ReactiveMarbles.Mvvm/
[CoreBadge]: https://img.shields.io/nuget/v/ReactiveMarbles.Mvvm.svg

## Get Started

### Registering Framework Concerns

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

### AsValue

`AsValue` allows you to bind an `IObservable<T>` to a property that produces a property changed event.

```csharp
_valueChange =
    this.WhenChanged(x => x.Property)
        .Select(x => x + "Changed")
        .AsValue(onChanged: x => RaisePropertyChanged(nameof(ValueChange)));
```

## Benchmarks

To see how Mvvm compares to other frameworks see: [Benchmarks](https://github.com/reactivemarbles/Mvvm/blob/main/src/ReactiveMarbles.Mvvm.Benchmarks/README.MD)
