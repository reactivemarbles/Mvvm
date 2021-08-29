## RxObject

|             Method | CreateNumber |           Mean |        Error |       StdDev |    Gen 0 |    Gen 1 | Gen 2 |   Allocated |
|------------------- |------------- |---------------:|-------------:|-------------:|---------:|---------:|------:|------------:|
|   RxObjectCreation |            1 |       206.5 ns |      2.15 ns |      2.01 ns |   0.0782 |        - |     - |       656 B |
| RxObjectWithChange |            1 |       280.2 ns |      5.19 ns |      4.85 ns |   0.0839 |        - |     - |       704 B |
|   RxObjectCreation |          100 |    12,782.4 ns |    212.66 ns |    188.52 ns |   6.1340 |   0.9308 |     - |    51,344 B |
| RxObjectWithChange |          100 |    17,117.2 ns |    318.94 ns |    282.74 ns |   6.6833 |   0.9155 |     - |    56,144 B |
|   RxObjectCreation |         4000 |   904,746.9 ns | 15,505.56 ns | 13,745.27 ns | 244.1406 | 112.3047 |     - | 2,048,154 B |
| RxObjectWithChange |         4000 | 1,269,486.4 ns | 24,327.69 ns | 22,756.13 ns | 267.5781 | 132.8125 |     - | 2,240,152 B |


## ReactiveUI ReactiveObject

|                   Method | CreateNumber |            Mean |         Error |        StdDev |    Gen 0 |    Gen 1 |    Gen 2 |   Allocated |
|------------------------- |------------- |----------------:|--------------:|--------------:|---------:|---------:|---------:|------------:|
|   ReactiveObjectCreation |            1 |        238.8 ns |       3.27 ns |       3.06 ns |   0.1068 |        - |        - |       896 B |
| ReactiveObjectWithChange |            1 |      1,530.3 ns |      30.56 ns |      40.79 ns |   0.2575 |   0.0057 |   0.0019 |     2,157 B |
|   ReactiveObjectCreation |          100 |     15,796.5 ns |     314.80 ns |     398.12 ns |   9.0027 |   1.7090 |        - |    75,344 B |
| ReactiveObjectWithChange |          100 |    132,292.6 ns |   2,553.90 ns |   3,580.21 ns |  24.1699 |   1.9531 |   0.7324 |   203,267 B |
|   ReactiveObjectCreation |         4000 |  1,530,059.8 ns |  26,466.29 ns |  24,756.59 ns | 359.3750 | 179.6875 |        - | 3,008,145 B |
| ReactiveObjectWithChange |         4000 | 11,304,993.4 ns | 146,890.54 ns | 137,401.50 ns | 953.1250 | 468.7500 | 218.7500 | 8,091,139 B |