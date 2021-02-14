# FSharp.IMCore


This repo hosts opinionated view on F# usage of System.Collections.Immutable

Benchmarks showed that sometimes System.Collections.Immutable datastructures can be faster than ones implemented in FSharp.Core.

This library tries to provide the same API as the standard collection (aside for the `IM` prefix),
in order to be able to simply replace i.e. `Set` with `IMSet` in the whole project and make no other changes - with some perfomance boost gained.



Benchmarks will be added below for the reference of which structures to use in which cases.


For the not opinionated bindings to System.Collection.Immutable, please check out https://github.com/fsprojects/FSharp.Collections.Immutable


<details>
<summary>Set vs IMSet</summary>

BenchmarkDotNet=v0.12.1, OS=macOS Catalina 10.15.4 (19E287) [Darwin 19.4.0]
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=5.0.101
[Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT DEBUG
DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT




|          Method |     N |              Mean |           Error |          StdDev |            Median |     Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|---------------- |------ |------------------:|----------------:|----------------:|------------------:|----------:|---------:|---------:|----------:|
|       SetForAll |  1000 |         14.700 ns |       0.2455 ns |       0.2050 ns |         14.752 ns |    0.0029 |        - |        - |      24 B |
|     IMSetForAll |  1000 |     29,054.930 ns |     615.0602 ns |   1,813.5189 ns |     28,251.453 ns |         - |        - |        - |      24 B |
|   SetDifference |  1000 |    524,132.267 ns |  10,119.2133 ns |  11,653.3011 ns |    523,768.420 ns |   55.6641 |   8.7891 |        - |  467080 B |
| IMSetDifference |  1000 |    174,949.721 ns |   3,250.3236 ns |   2,881.3272 ns |    174,564.877 ns |         - |        - |        - |         - |
|         SetFold |  1000 |      7,993.737 ns |     132.2576 ns |     157.4433 ns |      7,921.279 ns |         - |        - |        - |      24 B |
|       IMSetFold |  1000 |     33,549.818 ns |     657.3066 ns |     831.2824 ns |     33,344.952 ns |         - |        - |        - |      88 B |
|          SetMap |  1000 |    771,399.931 ns |  11,264.1887 ns |  16,154.7628 ns |    767,108.819 ns |   72.2656 |  23.4375 |        - |  612328 B |
|        IMSetMap |  1000 |    873,785.402 ns |  10,571.7963 ns |   9,371.6221 ns |    872,050.853 ns |   17.5781 |   5.8594 |        - |  152816 B |
|       SetFilter |  1000 |    218,700.975 ns |   3,205.7012 ns |   2,998.6149 ns |    218,443.311 ns |   27.0996 |   2.9297 |        - |  228160 B |
|     IMSetFilter |  1000 |     59,390.583 ns |     676.9961 ns |     600.1394 ns |     59,330.238 ns |    3.8452 |   0.4272 |        - |   32240 B |
|       SetForAll | 10000 |          9.706 ns |       0.2080 ns |       0.2043 ns |          9.682 ns |    0.0029 |        - |        - |      24 B |
|     IMSetForAll | 10000 |    266,011.907 ns |   5,206.9852 ns |   4,870.6172 ns |    265,458.884 ns |         - |        - |        - |      24 B |
|   SetDifference | 10000 |  8,270,620.653 ns | 164,144.7644 ns | 255,553.6228 ns |  8,267,830.445 ns |  734.3750 | 296.8750 |        - | 6247053 B |
| IMSetDifference | 10000 |  2,194,955.674 ns |  42,296.4607 ns |  54,997.3462 ns |  2,185,339.125 ns |         - |        - |        - |       1 B |
|         SetFold | 10000 |    116,751.321 ns |   2,271.0171 ns |   2,615.3066 ns |    116,162.927 ns |         - |        - |        - |      24 B |
|       IMSetFold | 10000 |    365,052.433 ns |   7,190.9664 ns |   9,350.2875 ns |    365,610.228 ns |         - |        - |        - |      88 B |
|          SetMap | 10000 | 14,713,954.222 ns | 216,661.3646 ns | 192,064.6564 ns | 14,688,465.070 ns | 1046.8750 | 468.7500 | 109.3750 | 7847356 B |
|        IMSetMap | 10000 | 12,298,302.701 ns | 233,818.5241 ns | 250,183.2011 ns | 12,269,631.336 ns |  187.5000 |  93.7500 |  31.2500 | 1622349 B |
|       SetFilter | 10000 |  3,731,749.163 ns |  65,027.2574 ns |  63,865.4617 ns |  3,736,295.391 ns |  382.8125 | 156.2500 |        - | 3230803 B |
|     IMSetFilter | 10000 |    657,238.891 ns |  11,280.3972 ns |  11,584.1374 ns |    652,125.737 ns |   43.9453 |  17.5781 |        - |  370736 B |

</details>