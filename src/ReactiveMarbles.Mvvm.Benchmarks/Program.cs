using BenchmarkDotNet.Running;

namespace ReactiveMarbles.Mvvm.Benchmarks
{
    /// <summary>
    /// Class which hosts the main entry point into the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point into the benchmarking application.
        /// </summary>
        /// <param name="args">Arguments from the command line.</param>
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
