// Copyright (c) 2019-2023 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// These are <see cref="AsValue{T,TObject}(System.IObservable{T?},TObject,string)"/> extension points for creating source generated type specific AsValue.
/// </summary>
public static class AsValueExtensionGeneratorExtensions
{
    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="sourceObject">The source object that will notify property change events.</param>
    /// <param name="propertyName">The property name associated to the change notification.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <typeparam name="TObject">The object type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T?> AsValue<T, TObject>(this IObservable<T?> source, TObject sourceObject, string propertyName)
        where TObject : RxObject => new ValueBinder<T?>(source, _ => sourceObject.RaisePropertyChanged(propertyName), initialValue: () => default);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="sourceObject">The source object that will notify property change events.</param>
    /// <param name="propertyName">The property name associated to the change notification.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <typeparam name="TObject">The object type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T> AsValue<T, TObject>(this IObservable<T> source, TObject sourceObject, string propertyName, Func<T> initialValue)
        where TObject : RxObject => new ValueBinder<T>(source,  _ => sourceObject.RaisePropertyChanged(propertyName), initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="sourceObject">The source object that will notify property change events.</param>
    /// <param name="propertyName">The property name associated to the change notification.</param>
    /// <param name="scheduler">The scheduler instance where the value will output.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <typeparam name="TObject">The object type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T> AsValue<T, TObject>(this IObservable<T> source, TObject sourceObject, string propertyName, IScheduler scheduler, Func<T> initialValue)
        where TObject : RxObject => new ValueBinder<T>(source, _ => sourceObject.RaisePropertyChanged(propertyName), scheduler, initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="sourceObject">The source object that will notify property change events.</param>
    /// <param name="propertyName">The property name associated to the change notification.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <param name="raiseChanging">A value that indicates whether to raise the changing event.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <typeparam name="TObject">The object type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T> AsValue<T, TObject>(this IObservable<T> source, TObject sourceObject, string propertyName, Func<T> initialValue, bool raiseChanging)
        where TObject : RxObject =>
        raiseChanging
            ? new ValueBinder<T>(source, _ => sourceObject.RaisePropertyChanging(propertyName), _ => sourceObject.RaisePropertyChanged(propertyName), initialValue)
            : new ValueBinder<T>(source, _ => sourceObject.RaisePropertyChanged(propertyName), initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="sourceObject">The source object that will notify property change events.</param>
    /// <param name="propertyName">The property name associated to the change notification.</param>
    /// <param name="scheduler">The scheduler instance where the value will output.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <param name="raiseChanging">A value that indicates whether to raise the changing event.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <typeparam name="TObject">The object type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T> AsValue<T, TObject>(this IObservable<T> source, TObject sourceObject, string propertyName, IScheduler scheduler, Func<T> initialValue, bool raiseChanging)
        where TObject : RxObject =>
        raiseChanging
            ? new ValueBinder<T>(source, _ => sourceObject.RaisePropertyChanging(propertyName), _ => sourceObject.RaisePropertyChanged(propertyName), scheduler, initialValue)
            : new ValueBinder<T>(source, _ => sourceObject.RaisePropertyChanged(propertyName), scheduler, initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// This value is lazy and will not subscribe until first access.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="sourceObject">The source object that will notify property change events.</param>
    /// <param name="propertyName">The property name associated to the change notification.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <typeparam name="TObject">The object type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T?> AsLazyValue<T, TObject>(this IObservable<T?> source, TObject sourceObject, string propertyName)
        where TObject : RxObject => new LazyValueBinder<T?>(source, _ => sourceObject.RaisePropertyChanged(propertyName), initialValue: () => default);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// This value is lazy and will not subscribe until first access.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="sourceObject">The source object that will notify property change events.</param>
    /// <param name="propertyName">The property name associated to the change notification.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <typeparam name="TObject">The object type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T> AsLazyValue<T, TObject>(this IObservable<T> source, TObject sourceObject, string propertyName, Func<T> initialValue)
        where TObject : RxObject => new LazyValueBinder<T>(source, _ => sourceObject.RaisePropertyChanged(propertyName), initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// This value is lazy and will not subscribe until first access.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="sourceObject">The source object that will notify property change events.</param>
    /// <param name="propertyName">The property name associated to the change notification.</param>
    /// <param name="scheduler">The scheduler instance where the value will output.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <typeparam name="TObject">The object type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T> AsLazyValue<T, TObject>(this IObservable<T> source, TObject sourceObject, string propertyName, IScheduler scheduler, Func<T> initialValue)
        where TObject : RxObject => new LazyValueBinder<T>(source, _ => sourceObject.RaisePropertyChanged(propertyName), scheduler, initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// This value is lazy and will not subscribe until first access.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanging">Callback with the changing value.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T> AsLazyValue<T>(this IObservable<T> source, Action<T?> onChanging, Action<T?> onChanged, Func<T> initialValue) =>
        new LazyValueBinder<T>(source, onChanging, onChanged, initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// This value is lazy and will not subscribe until first access.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanging">Callback with the changing value.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <param name="scheduler">The scheduler instance where the value will output.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static IValueBinder<T> AsLazyValue<T>(this IObservable<T> source, Action<T?> onChanging, Action<T> onChanged, IScheduler scheduler, Func<T> initialValue) =>
        new LazyValueBinder<T>(source, onChanging, onChanged, scheduler, initialValue);
}
