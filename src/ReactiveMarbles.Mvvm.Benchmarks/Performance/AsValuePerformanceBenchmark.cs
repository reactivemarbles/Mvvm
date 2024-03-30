// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

using ReactiveMarbles.Mvvm.Benchmarks.Memory;
using ReactiveMarbles.PropertyChanged;

namespace ReactiveMarbles.Mvvm.Benchmarks.Performance;

/// <summary>
/// Benchmark for the AsValue.
/// </summary>
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class AsValuePerformanceBenchmark
{
    /// <summary>
    /// Benchmarks AsValue.
    /// </summary>
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Performance")]
    public void AsValueBenchmark()
    {
        DummyRxObject thing = new();
        var unused = thing
            .WhenChanged(x => x.NotSerialized, x => x.IsOnlyOneWord, (not, one) => not + one)
            .AsValue(onChanged: _ => { });
    }

    /// <summary>
    /// Benchmarks AsValue when word changes.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Performance")]
    public void AsValueWhenWordChangedBenchmark()
    {
        _ = new DummyRxObject
        {
            IsOnlyOneWord = "Two Words",
        };
    }
}