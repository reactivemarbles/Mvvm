using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace ReactiveMarbles.Mvvm.Benchmarks.Memory
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser()]
    [MarkdownExporterAttribute.GitHub]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class RxObjectMemoryBenchmark
    {
        [Params(1, 100, 4000)] public int CreateNumber;

        [Benchmark]
        [BenchmarkCategory("Memory")]
        public void RxObjectCreation()
        {
            var thing = Enumerable.Range(0, CreateNumber)
                .Select(x => new DummyRxObject())
                .ToList();
        }

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
}