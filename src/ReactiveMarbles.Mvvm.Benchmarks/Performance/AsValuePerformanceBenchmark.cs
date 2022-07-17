using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using ReactiveMarbles.Mvvm.Benchmarks.Memory;
using ReactiveMarbles.PropertyChanged;
using ReactiveUI;

namespace ReactiveMarbles.Mvvm.Benchmarks.Performance
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class AsValuePerformanceBenchmark
    {
        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Performance")]
        public void AsValueBenchmark()
        {
            var thing = new DummyRxObject();
            var sut = thing.WhenChanged(x => x.NotSerialized, x => x.IsOnlyOneWord, (not, one) => not + one).AsValue(onChanged: _ => { });
        }

        [Benchmark]
        [BenchmarkCategory("Performance")]
        public void AsValueWhenWordChangedBenchmark()
        {
            var thing = new DummyRxObject();
            thing.IsOnlyOneWord = "Two Words";
        }

        [Benchmark]
        [BenchmarkCategory("Performance")]
        public void ToPropertyBenchmark()
        {
            var thing = new DummyReactiveObject();
            var sut = thing.WhenChanged(x => x.NotSerialized, x => x.IsOnlyOneWord, (not, one) => not + one).ToProperty(thing, x => x.ObservableProperty);
        }

        [Benchmark]
        [BenchmarkCategory("Performance")]
        public void ToPropertyWhenWordChangedBenchmark()
        {
            var thing = new DummyReactiveObject();
            thing.IsOnlyOneWord = "Two Words";
        }
    }
}