// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;

namespace ReactiveMarbles.Mvvm;

/// <summary>
/// Extensions methods that provider a binder implementation for observable values.
/// </summary>
public static class AsValueExtensions
{
    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static ValueBinder<T?> AsValue<T>(this IObservable<T?> source, Action<T?> onChanged) =>
        new(source, onChanged, initialValue: () => default);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static ValueBinder<T> AsValue<T>(this IObservable<T> source, Action<T> onChanged, Func<T> initialValue) =>
        new(source, onChanged, initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <param name="scheduler">The scheduler instance where the value will output.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static ValueBinder<T> AsValue<T>(this IObservable<T> source, Action<T> onChanged, IScheduler scheduler, Func<T> initialValue) =>
        new(source, onChanged, scheduler, initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanging">Callback with the changing value.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static ValueBinder<T> AsValue<T>(this IObservable<T> source, Action<T?> onChanging, Action<T> onChanged, Func<T> initialValue) =>
        new(source, onChanging, onChanged, initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanging">Callback with the changing value.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <param name="scheduler">The scheduler instance where the value will output.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static ValueBinder<T> AsValue<T>(this IObservable<T> source, Action<T?> onChanging, Action<T> onChanged, IScheduler scheduler, Func<T> initialValue) =>
        new(source, onChanging, onChanged, scheduler, initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// This value is lazy and will not subscribe until first access.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static LazyValueBinder<T?> AsLazyValue<T>(this IObservable<T?> source, Action<T?> onChanged) =>
        new(source, onChanged, initialValue: () => default);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// This value is lazy and will not subscribe until first access.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static LazyValueBinder<T> AsLazyValue<T>(this IObservable<T> source, Action<T> onChanged, Func<T> initialValue) =>
        new(source, onChanged, initialValue);

    /// <summary>
    /// Projects an observable value to a property for binding.
    /// This value is lazy and will not subscribe until first access.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onChanged">Callback with the changed value.</param>
    /// <param name="scheduler">The scheduler instance where the value will output.</param>
    /// <param name="initialValue">The function that provides the initial value.</param>
    /// <typeparam name="T">The property type.</typeparam>
    /// <returns>A binder.</returns>
    public static LazyValueBinder<T> AsLazyValue<T>(this IObservable<T> source, Action<T?> onChanged, IScheduler scheduler, Func<T> initialValue) =>
        new(source, onChanged, scheduler, initialValue);

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
    public static LazyValueBinder<T> AsLazyValue<T>(this IObservable<T> source, Action<T?> onChanging, Action<T?> onChanged, Func<T> initialValue) =>
        new(source, onChanging, onChanged, initialValue);

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
    public static LazyValueBinder<T> AsLazyValue<T>(this IObservable<T> source, Action<T?> onChanging, Action<T> onChanged, IScheduler scheduler, Func<T> initialValue) =>
        new(source, onChanging, onChanged, scheduler, initialValue);
}
