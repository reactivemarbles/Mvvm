// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reactive.Disposables;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using DynamicData.Binding;
using ReactiveMarbles.PropertyChanged;
using ReactiveUI;

namespace ReactiveMarbles.Mvvm.Benchmarks.Memory;

/// <summary>
/// Benchmark for the reactive object.
/// </summary>
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ReactiveObjectMemoryBenchmark
{
    /// <summary>
    /// Gets or sets a parameter for the number of creations.
    /// </summary>
    [Params(1, 100, 4000)]
    public int CreateNumber { get; set; }

    /// <summary>
    /// Benchmarks the object creation.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Memory")]
    public void ReactiveObjectCreation()
    {
        var thing =
            Enumerable.Range(0, CreateNumber)
                .Select(x => new DummyReactiveObject())
                .ToList();
    }

    /// <summary>
    /// Benchmarks the object value changes.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Memory")]
    public void ReactiveObjectWithChange()
    {
        var thing = Enumerable.Range(0, CreateNumber)
            .Select(x => new DummyReactiveObject())
            .ToList();

        foreach (var dummy in thing)
        {
            dummy.IsNotNullString = "New";
        }
    }

    /// <summary>
    /// A benchmark for reactive object and when any value subscription.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Memory")]
    public void ReactiveObjectWhenAnyValueWithSubscribe()
    {
        var thing = Enumerable.Range(0, CreateNumber)
            .Select(x => new DummyReactiveObject())
            .ToList();

        foreach (var dummy in thing)
        {
            dummy.WhenAnyValue(x => x.IsNotNullString).Subscribe();
        }
    }

    /// <summary>
    /// A benchmark for reactive object and when any value subscription and disposal.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Memory")]
    public void ReactiveObjectWhenAnyValueWithSubscribeAndDispose()
    {
        var disposables = new CompositeDisposable();
        var thing = Enumerable.Range(0, CreateNumber)
            .Select(x => new DummyReactiveObject())
            .ToList();

        foreach (var dummy in thing)
        {
            dummy.WhenAnyValue(x => x.IsNotNullString).Subscribe().DisposeWith(disposables);
        }
    }

    /// <summary>
    /// A benchmark for reactive object and when change subscription.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Memory")]
    public void ReactiveObjectWhenChangedWithSubscribe()
    {
        var thing = Enumerable.Range(0, CreateNumber)
            .Select(x => new DummyReactiveObject())
            .ToList();

        foreach (var dummy in thing)
        {
            dummy.WhenChanged(x => x.IsNotNullString).Subscribe();
        }
    }

    /// <summary>
    /// A benchmark for reactive object and when changed subscription.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Memory")]
    public void ReactiveObjectWhenChangedWithSubscribeAndDispose()
    {
        var disposables = new CompositeDisposable();
        var thing = Enumerable.Range(0, CreateNumber)
            .Select(x => new DummyReactiveObject())
            .ToList();

        foreach (var dummy in thing)
        {
            dummy.WhenChanged(x => x.IsNotNullString).Subscribe().DisposeWith(disposables);
        }
    }
}