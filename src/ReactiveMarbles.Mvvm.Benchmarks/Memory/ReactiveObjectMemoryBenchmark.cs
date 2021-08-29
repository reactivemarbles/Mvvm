using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace ReactiveMarbles.Mvvm.Benchmarks.Memory
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class ReactiveObjectMemoryBenchmark
    {
        [Params(1, 100, 4000)] public int CreateNumber;

        [Benchmark]
        [BenchmarkCategory("Memory")]
        public void ReactiveObjectCreation()
        {
            var thing = Enumerable.Range(0, CreateNumber)
                .Select(x => new DummyReactiveObject())
                .ToList();
        }

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
    }
}