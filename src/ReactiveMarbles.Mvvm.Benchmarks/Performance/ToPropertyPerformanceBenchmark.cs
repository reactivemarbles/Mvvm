// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

using ReactiveMarbles.Mvvm.Benchmarks.Memory;
using ReactiveMarbles.PropertyChanged;

using ReactiveUI;

namespace ReactiveMarbles.Mvvm.Benchmarks.Performance;

/// <summary>
/// Benchmark for the ToProperty.
/// </summary>
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ToPropertyPerformanceBenchmark
{
    /// <summary>
    /// Benchmarks ToProperty performance.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Performance")]
    public void ToPropertyBenchmark()
    {
        DummyReactiveObject thing = new();
        var unused =
            thing
                .WhenChanged(x => x.NotSerialized, x => x.IsOnlyOneWord, (not, one) => not + one)
            .ToProperty(thing, x => x.ObservableProperty);
    }

    /// <summary>
    /// Benchmarks ToProperty when a word changes.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Performance")]
    public void ToPropertyWhenWordChangedBenchmark()
    {
        _ = new DummyReactiveObject
        {
            IsOnlyOneWord = "Two Words",
        };
    }
}