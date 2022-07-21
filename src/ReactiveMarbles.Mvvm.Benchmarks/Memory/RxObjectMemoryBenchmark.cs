// Copyright (c) 2019-2022 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace ReactiveMarbles.Mvvm.Benchmarks.Memory;

/// <summary>
/// Benchmark for the RxObject.
/// </summary>
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class RxObjectMemoryBenchmark
{
    /// <summary>
    /// Gets or sets a parameter for how many numbers to create.
    /// </summary>
    [Params(1, 100, 4000)]
    public int CreateNumber { get; set; }

    /// <summary>
    /// A benchmark for the object creation time.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Memory")]
    public void RxObjectCreation()
    {
        var thing = Enumerable.Range(0, CreateNumber)
            .Select(x => new DummyRxObject())
            .ToList();
    }

    /// <summary>
    /// A benchmark for the object change time.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Memory")]
    public void RxObjectWithChange()
    {
        var thing = Enumerable.Range(0, CreateNumber)
            .Select(x => new DummyRxObject())
            .ToList();

        foreach (var dummy in thing)
        {
            dummy.IsNotNullString = "New";
        }
    }
}