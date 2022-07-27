``` ini

BenchmarkDotNet=v0.13.1, OS=macOS Monterey 12.1 (21C52) [Darwin 21.2.0]
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]   : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  .NET 6.0 : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=.NET 6.0  Runtime=.NET 6.0  

```

# ReactiveMarbles

## RxObject
|                                      Method | CreateNumber |             Mean |           Error |           StdDev |  Ratio | RatioSD |     Gen 0 |     Gen 1 |     Gen 2 |    Allocated |
|-------------------------------------------- |------------- |-----------------:|----------------:|-----------------:|-------:|--------:|----------:|----------:|----------:|-------------:|
|                        **SubjectWithSubscribe** |            **1** |         **163.8 ns** |         **0.80 ns** |          **0.67 ns** |   **1.00** |    **0.00** |    **0.0353** |         **-** |         **-** |        **296 B** |
|                            RxObjectCreation |            1 |      15,404.8 ns |       132.73 ns |        124.16 ns |  94.02 |    1.04 |    0.8240 |    0.1221 |         - |      7,001 B |
|                          RxObjectWithChange |            1 |      16,066.7 ns |       125.85 ns |        111.57 ns |  98.04 |    0.84 |    0.8240 |    0.1221 |         - |      7,049 B |
|              SubjectWithSubscribeAndDispose |            1 |         261.4 ns |         2.52 ns |          2.36 ns |   1.59 |    0.01 |    0.0534 |         - |         - |        448 B |
|           RxObjectWhenAnyValueWithSubscribe |            1 |      34,874.8 ns |       375.44 ns |        332.82 ns | 212.98 |    1.79 |    1.4038 |    0.2441 |         - |     12,122 B |
| RxObjectWhenAnyValueWithSubscribeAndDispose |            1 |      35,340.0 ns |       664.53 ns |        711.04 ns | 216.44 |    5.44 |    1.4648 |    0.2441 |         - |     12,274 B |
|            RxObjectWhenChangedWithSubscribe |            1 |      24,372.1 ns |       152.84 ns |        135.49 ns | 148.72 |    0.98 |    1.1597 |    0.2136 |         - |      9,912 B |
|  RxObjectWhenChangedWithSubscribeAndDispose |            1 |      24,027.8 ns |       152.85 ns |        135.50 ns | 146.63 |    0.85 |    1.1902 |    0.1831 |         - |     10,066 B |
|                                             |              |                  |                 |                  |        |         |           |           |           |              |
|                        **SubjectWithSubscribe** |          **100** |       **8,490.2 ns** |        **22.88 ns** |         **19.11 ns** |   **1.00** |    **0.00** |    **1.8311** |    **0.0916** |         **-** |     **15,344 B** |
|                            RxObjectCreation |          100 |   1,601,101.5 ns |     7,304.69 ns |      6,099.75 ns | 188.58 |    0.89 |   80.0781 |   31.2500 |         - |    685,594 B |
|                          RxObjectWithChange |          100 |   1,579,546.5 ns |    11,572.24 ns |     10,824.68 ns | 185.91 |    1.28 |   82.0313 |   35.1563 |         - |    690,462 B |
|              SubjectWithSubscribeAndDispose |          100 |      12,877.0 ns |        30.04 ns |         25.08 ns |   1.52 |    0.01 |    2.0905 |    0.1068 |         - |     17,600 B |
|           RxObjectWhenAnyValueWithSubscribe |          100 |   2,942,318.0 ns |    25,730.88 ns |     24,068.68 ns | 346.52 |    3.47 |  140.6250 |   66.4063 |         - |  1,197,315 B |
| RxObjectWhenAnyValueWithSubscribeAndDispose |          100 |   3,043,304.2 ns |    16,970.28 ns |     15,874.01 ns | 358.35 |    2.17 |  140.6250 |   70.3125 |         - |  1,199,888 B |
|            RxObjectWhenChangedWithSubscribe |          100 |   2,412,206.3 ns |    20,327.44 ns |     19,014.30 ns | 284.45 |    2.21 |  113.2813 |   42.9688 |         - |    976,616 B |
|  RxObjectWhenChangedWithSubscribeAndDispose |          100 |   2,293,934.2 ns |    13,820.20 ns |     12,927.42 ns | 270.23 |    1.50 |  113.2813 |   54.6875 |         - |    978,872 B |
|                                             |              |                  |                 |                  |        |         |           |           |           |              |
|                        **SubjectWithSubscribe** |         **4000** |     **395,955.1 ns** |     **2,439.15 ns** |      **2,162.24 ns** |   **1.00** |    **0.00** |   **72.2656** |   **31.7383** |         **-** |    **608,144 B** |
|                            RxObjectCreation |         4000 | 121,556,272.3 ns | 1,393,992.12 ns |  1,303,941.09 ns | 306.69 |    4.38 | 3600.0000 | 1400.0000 |  400.0000 | 27,429,170 B |
|                          RxObjectWithChange |         4000 | 133,998,168.1 ns | 2,663,042.36 ns |  6,172,005.62 ns | 331.52 |   19.73 | 3750.0000 | 1500.0000 |  500.0000 | 27,622,806 B |
|              SubjectWithSubscribeAndDispose |         4000 |     593,221.8 ns |    11,081.81 ns |     11,380.20 ns |   1.50 |    0.03 |   80.0781 |   33.2031 |         - |    674,009 B |
|           RxObjectWhenAnyValueWithSubscribe |         4000 | 249,498,696.7 ns | 4,984,701.60 ns |  3,891,727.13 ns | 630.13 |   11.25 | 6500.0000 | 3000.0000 | 1000.0000 | 47,911,968 B |
| RxObjectWhenAnyValueWithSubscribeAndDispose |         4000 | 246,374,019.5 ns | 4,739,604.23 ns | 12,149,417.59 ns | 621.43 |   28.42 | 6500.0000 | 3000.0000 | 1000.0000 | 47,976,152 B |
|            RxObjectWhenChangedWithSubscribe |         4000 | 136,864,129.7 ns | 2,564,240.52 ns |  2,398,592.17 ns | 344.97 |    6.14 | 4000.0000 | 1000.0000 |         - | 39,026,848 B |
|  RxObjectWhenChangedWithSubscribeAndDispose |         4000 | 139,549,864.6 ns | 1,878,092.90 ns |  1,664,880.44 ns | 352.44 |    4.24 | 4000.0000 | 1000.0000 |         - | 39,173,744 B |

## AsValue

|                             Method |      Mean |    Error |   StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|----------------------------------- |----------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|
|                   AsValueBenchmark |  48.78 μs | 0.257 μs | 0.241 μs |  1.00 |    0.00 | 1.8311 | 0.3662 |     15 KB |
|    AsValueWhenWordChangedBenchmark |  20.54 μs | 0.096 μs | 0.090 μs |  0.42 |    0.00 | 0.7935 | 0.1221 |      7 KB |


# ReactiveUI

## ReactiveObject
|                                            Method | CreateNumber |          Mean |        Error |        StdDev |        Median |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|-------------------------------------------------- |------------- |--------------:|-------------:|--------------:|--------------:|----------:|----------:|----------:|----------:|
|                            **ReactiveObjectCreation** |            **1** |      **35.28 μs** |     **0.628 μs** |      **0.838 μs** |      **35.01 μs** |    **1.5259** |    **0.1831** |    **0.0610** |     **12 KB** |
|                          ReactiveObjectWithChange |            1 |      35.50 μs |     0.639 μs |      0.534 μs |      35.63 μs |    1.5259 |    0.1831 |    0.0610 |     13 KB |
|           ReactiveObjectWhenAnyValueWithSubscribe |            1 |      47.36 μs |     0.918 μs |      1.057 μs |      46.83 μs |    2.0142 |    0.3662 |    0.1221 |     16 KB |
| ReactiveObjectWhenAnyValueWithSubscribeAndDispose |            1 |      46.78 μs |     0.342 μs |      0.320 μs |      46.78 μs |    2.0142 |    0.3662 |    0.1221 |     17 KB |
|            ReactiveObjectWhenChangedWithSubscribe |            1 |      55.65 μs |     0.338 μs |      0.316 μs |      55.68 μs |    2.0752 |    0.3662 |    0.1221 |     17 KB |
|  ReactiveObjectWhenChangedWithSubscribeAndDispose |            1 |      54.58 μs |     0.340 μs |      0.318 μs |      54.66 μs |    2.1362 |    0.3662 |    0.1221 |     18 KB |
|                            **ReactiveObjectCreation** |          **100** |   **3,734.45 μs** |    **29.458 μs** |     **24.599 μs** |   **3,734.25 μs** |  **148.4375** |   **70.3125** |   **15.6250** |  **1,234 KB** |
|                          ReactiveObjectWithChange |          100 |   3,796.80 μs |    75.653 μs |    119.994 μs |   3,776.35 μs |  152.3438 |   66.4063 |   15.6250 |  1,266 KB |
|           ReactiveObjectWhenAnyValueWithSubscribe |          100 |   5,070.60 μs |    99.147 μs |    286.062 μs |   4,988.22 μs |  195.3125 |   78.1250 |   15.6250 |  1,633 KB |
| ReactiveObjectWhenAnyValueWithSubscribeAndDispose |          100 |   5,230.51 μs |    72.298 μs |     64.090 μs |   5,230.02 μs |  195.3125 |   93.7500 |   39.0625 |  1,637 KB |
|            ReactiveObjectWhenChangedWithSubscribe |          100 |   5,291.38 μs |    87.529 μs |     77.592 μs |   5,306.46 μs |  210.9375 |  101.5625 |   39.0625 |  1,723 KB |
|  ReactiveObjectWhenChangedWithSubscribeAndDispose |          100 |   5,160.44 μs |    39.409 μs |     32.908 μs |   5,153.08 μs |  210.9375 |  101.5625 |   31.2500 |  1,726 KB |
|                            **ReactiveObjectCreation** |         **4000** | **274,614.27 μs** | **3,976.521 μs** |  **4,419.892 μs** | **272,998.57 μs** | **7000.0000** | **3000.0000** | **1000.0000** | **49,307 KB** |
|                          ReactiveObjectWithChange |         4000 | 262,808.06 μs | 5,248.733 μs |  5,154.958 μs | 262,669.73 μs | 7000.0000 | 3000.0000 | 1000.0000 | 50,742 KB |
|           ReactiveObjectWhenAnyValueWithSubscribe |         4000 | 371,405.46 μs | 7,269.827 μs | 12,922.110 μs | 368,357.67 μs | 9000.0000 | 4000.0000 | 1000.0000 | 65,274 KB |
| ReactiveObjectWhenAnyValueWithSubscribeAndDispose |         4000 | 342,062.91 μs | 8,536.027 μs | 25,168.668 μs | 344,686.07 μs | 9000.0000 | 4000.0000 | 1000.0000 | 65,490 KB |
|            ReactiveObjectWhenChangedWithSubscribe |         4000 | 371,596.61 μs | 7,155.489 μs | 10,927.188 μs | 375,727.41 μs | 9000.0000 | 4000.0000 | 1000.0000 | 68,876 KB |
|  ReactiveObjectWhenChangedWithSubscribeAndDispose |         4000 | 369,832.81 μs | 7,322.658 μs | 10,501.937 μs | 374,104.44 μs | 9000.0000 | 4000.0000 | 1000.0000 | 68,941 KB |


## ToProperty
|                             Method |      Mean |    Error |   StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|----------------------------------- |----------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|
|                ToPropertyBenchmark | 105.46 μs | 0.971 μs | 0.860 μs |  2.16 |    0.02 | 2.9297 | 0.4883 |     25 KB |
| ToPropertyWhenWordChangedBenchmark |  54.09 μs | 1.078 μs | 1.580 μs |  1.12 |    0.03 | 1.5869 | 0.2441 |     13 KB |