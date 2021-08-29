## RxObject

|             Method | CreateNumber |           Mean |         Error |        StdDev |         Median |    Gen 0 |    Gen 1 | Gen 2 | Allocated |
|------------------- |------------- |---------------:|--------------:|--------------:|---------------:|---------:|---------:|------:|----------:|
|   RxObjectCreation |            1 |       564.8 ns |      10.96 ns |      14.25 ns |       566.5 ns |   0.2022 |   0.0010 |     - |      2 KB |
| RxObjectWithChange |            1 |       647.4 ns |       9.88 ns |       8.76 ns |       647.0 ns |   0.2079 |   0.0010 |     - |      2 KB |
|   RxObjectCreation |          100 |    50,249.3 ns |     917.04 ns |     857.80 ns |    50,184.9 ns |  18.5547 |   5.7983 |     - |    152 KB |
| RxObjectWithChange |          100 |    56,502.8 ns |     911.23 ns |     852.37 ns |    56,459.0 ns |  19.1040 |   5.1270 |     - |    156 KB |
|   RxObjectCreation |         4000 | 5,593,739.2 ns |  69,053.41 ns |  64,592.60 ns | 5,600,920.0 ns | 742.1875 | 367.1875 |     - |  6,063 KB |
| RxObjectWithChange |         4000 | 6,234,991.0 ns | 122,895.87 ns | 224,722.04 ns | 6,133,371.3 ns | 757.8125 | 375.0000 |     - |  6,250 KB |


## ReactiveUI ReactiveObject
|                   Method | CreateNumber |            Mean |         Error |        StdDev |    Gen 0 |    Gen 1 |    Gen 2 |   Allocated |
|------------------------- |------------- |----------------:|--------------:|--------------:|---------:|---------:|---------:|------------:|
|   ReactiveObjectCreation |            1 |        242.4 ns |       4.57 ns |       6.26 ns |   0.1068 |        - |        - |       896 B |
| ReactiveObjectWithChange |            1 |      1,536.5 ns |      29.92 ns |      42.91 ns |   0.2575 |   0.0057 |   0.0019 |     2,157 B |
|   ReactiveObjectCreation |          100 |     16,884.4 ns |     335.55 ns |     560.64 ns |   9.0027 |   1.7090 |        - |    75,344 B |
| ReactiveObjectWithChange |          100 |    133,628.0 ns |   2,625.84 ns |   4,598.94 ns |  24.4141 |   1.7090 |   0.4883 |   203,267 B |
|   ReactiveObjectCreation |         4000 |  1,579,677.4 ns |  30,944.54 ns |  34,394.78 ns | 359.3750 | 179.6875 |        - | 3,008,163 B |
| ReactiveObjectWithChange |         4000 | 11,459,659.3 ns | 227,605.87 ns | 212,902.68 ns | 953.1250 | 468.7500 | 218.7500 | 8,091,142 B |
