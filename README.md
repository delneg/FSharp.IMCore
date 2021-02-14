# FSharp.IMCore


This repo hosts opinionated view on F# usage of System.Collections.Immutable

Benchmarks showed that sometimes System.Collections.Immutable datastructures can be faster than ones implemented in FSharp.Core.

This library tries to provide the same API as the standard collection (aside for the `IM` prefix),
in order to be able to simply replace i.e. `Set` with `IMSet` in the whole project and make no other changes - with some perfomance boost gained.

As of right now, the project is in very early stage - contributions are welcome, and please use with caution.
I.e. IMMap vs Map perfomance is relatively poor for most cases (measured for .net 5)


Benchmarks will be added below for the reference of which structures to use in which cases.


For the not opinionated bindings to System.Collection.Immutable, please check out https://github.com/fsprojects/FSharp.Collections.Immutable


<details>
<summary>Set vs IMSet</summary>

BenchmarkDotNet=v0.12.1, OS=macOS Catalina 10.15.4 (19E287) [Darwin 19.4.0]
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=5.0.101
[Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT DEBUG
DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT




|          Method |     N |             Mean |          Error |         StdDev |     Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|---------------- |------ |-----------------:|---------------:|---------------:|----------:|---------:|---------:|----------:|
|       SetForAll |  1000 |         15.12 ns |       0.318 ns |       0.297 ns |    0.0029 |        - |        - |      24 B |
|     IMSetForAll |  1000 |        202.19 ns |       3.077 ns |       2.728 ns |    0.0105 |        - |        - |      88 B |
|   SetDifference |  1000 |    535,945.47 ns |  10,392.899 ns |   9,213.034 ns |   54.6875 |   8.7891 |        - |  457960 B |
| IMSetDifference |  1000 |    189,834.97 ns |   2,910.793 ns |   2,858.788 ns |         - |        - |        - |         - |
|         SetFold |  1000 |      7,788.45 ns |     154.070 ns |     144.117 ns |         - |        - |        - |      24 B |
|       IMSetFold |  1000 |     33,523.88 ns |     494.207 ns |     462.282 ns |         - |        - |        - |      88 B |
|          SetMap |  1000 |    804,234.48 ns |  11,254.397 ns |   9,976.730 ns |   74.2188 |  24.4141 |        - |  622216 B |
|        IMSetMap |  1000 |    864,723.44 ns |   7,671.285 ns |   6,800.394 ns |   17.5781 |   5.8594 |        - |  152840 B |
|       SetFilter |  1000 |    232,328.91 ns |   3,842.689 ns |   3,208.819 ns |   28.3203 |   3.1738 |        - |  237808 B |
|     IMSetFilter |  1000 |     59,881.92 ns |     868.884 ns |     812.754 ns |    3.9063 |   0.4272 |        - |   32768 B |
|        SetUnion |  1000 |    251,169.27 ns |   4,960.091 ns |   4,639.672 ns |   35.6445 |   9.2773 |        - |  299600 B |
|      IMSetUnion |  1000 |    332,915.04 ns |   3,517.447 ns |   2,746.191 ns |   10.7422 |   3.4180 |        - |   90224 B |
|    SetIntersect |  1000 |     98,232.54 ns |   1,275.244 ns |   1,130.470 ns |         - |        - |        - |      40 B |
|  IMSetIntersect |  1000 |     92,270.31 ns |   1,779.152 ns |   1,903.673 ns |         - |        - |        - |         - |
|    SetSingleton |  1000 |         36.56 ns |       0.776 ns |       0.725 ns |    0.0114 |        - |        - |      96 B |
|  IMSetSingleton |  1000 |         41.35 ns |       0.806 ns |       0.673 ns |    0.0134 |        - |        - |     112 B |
|       SetMinMax |  1000 |        173.35 ns |       3.127 ns |       2.772 ns |    0.0172 |        - |        - |     144 B |
|     IMSetMinMax |  1000 |         31.06 ns |       0.685 ns |       0.733 ns |    0.0057 |        - |        - |      48 B |
|       SetForAll | 10000 |         15.06 ns |       0.284 ns |       0.266 ns |    0.0029 |        - |        - |      24 B |
|     IMSetForAll | 10000 |        225.82 ns |       3.406 ns |       3.020 ns |    0.0105 |        - |        - |      88 B |
|   SetDifference | 10000 |  8,735,941.96 ns | 173,008.386 ns | 177,666.874 ns |  734.3750 | 328.1250 |        - | 6253869 B |
| IMSetDifference | 10000 |  2,337,930.35 ns |  44,497.152 ns |  43,702.153 ns |         - |        - |        - |       1 B |
|         SetFold | 10000 |    116,106.35 ns |   2,038.932 ns |   3,406.597 ns |         - |        - |        - |      24 B |
|       IMSetFold | 10000 |    373,007.89 ns |   7,325.718 ns |   8,436.308 ns |         - |        - |        - |      88 B |
|          SetMap | 10000 | 15,296,230.38 ns | 248,434.323 ns | 232,385.619 ns | 1062.5000 | 500.0000 | 125.0000 | 7848734 B |
|        IMSetMap | 10000 | 12,329,599.39 ns | 189,904.430 ns | 168,345.331 ns |  187.5000 |  93.7500 |  31.2500 | 1622274 B |
|       SetFilter | 10000 |  4,097,716.27 ns |  79,736.462 ns |  81,883.476 ns |  390.6250 | 187.5000 |        - | 3308803 B |
|     IMSetFilter | 10000 |    665,409.43 ns |  12,099.531 ns |  11,317.908 ns |   43.9453 |  21.4844 |        - |  373040 B |
|        SetUnion | 10000 |  4,329,789.21 ns |  79,540.832 ns |  70,510.876 ns |  351.5625 | 171.8750 |        - | 3004979 B |
|      IMSetUnion | 10000 |  4,481,053.54 ns |  63,908.080 ns |  56,652.849 ns |  101.5625 |  46.8750 |        - |  897779 B |
|    SetIntersect | 10000 |  1,240,591.25 ns |  21,593.737 ns |  19,142.286 ns |         - |        - |        - |      41 B |
|  IMSetIntersect | 10000 |  1,045,768.42 ns |  19,914.963 ns |  19,559.156 ns |         - |        - |        - |       1 B |
|    SetSingleton | 10000 |         37.36 ns |       0.782 ns |       0.768 ns |    0.0114 |        - |        - |      96 B |
|  IMSetSingleton | 10000 |         43.65 ns |       0.755 ns |       0.706 ns |    0.0134 |        - |        - |     112 B |
|       SetMinMax | 10000 |        223.46 ns |       3.229 ns |       3.020 ns |    0.0172 |        - |        - |     144 B |
|     IMSetMinMax | 10000 |         42.67 ns |       0.354 ns |       0.296 ns |    0.0057 |        - |        - |      48 B |


</details>

<details>
<summary>Map vs IMMap</summary>


BenchmarkDotNet=v0.12.1, OS=macOS Catalina 10.15.4 (19E287) [Darwin 19.4.0]
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT DEBUG
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT



|      Method |     N |           Mean |        Error |       StdDev |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|------------ |------ |---------------:|-------------:|-------------:|--------:|--------:|------:|----------:|
|   MapForAll |  1000 |       118.5 ns |      2.20 ns |      3.23 ns |  0.0029 |       - |     - |      24 B |
| IMMapForAll |  1000 |       693.0 ns |     12.80 ns |     11.35 ns |  0.0238 |       - |     - |     200 B |
|     MapFold |  1000 |     7,063.5 ns |    135.06 ns |    144.52 ns |       - |       - |     - |      24 B |
|   IMMapFold |  1000 |    66,275.8 ns |  1,304.61 ns |  1,553.05 ns |       - |       - |     - |      24 B |
|      MapMap |  1000 |    58,733.6 ns |  1,127.82 ns |  1,206.76 ns | 10.2539 |  2.5635 |     - |   85856 B |
|    IMMapMap |  1000 |   322,247.6 ns |  6,232.85 ns |  5,830.22 ns | 10.7422 |  2.9297 |     - |   93576 B |
|   MapFilter |  1000 |    32,976.7 ns |    646.65 ns |    718.75 ns |  2.4414 |       - |     - |   20880 B |
| IMMapFilter |  1000 |    72,204.2 ns |  1,422.96 ns |  1,331.04 ns |       - |       - |     - |      80 B |
|   MapForAll | 10000 |       133.9 ns |      2.60 ns |      3.97 ns |  0.0029 |       - |     - |      24 B |
| IMMapForAll | 10000 |       639.4 ns |     11.05 ns |      9.80 ns |  0.0238 |       - |     - |     200 B |
|     MapFold | 10000 |    27,575.8 ns |    443.66 ns |    370.47 ns |       - |       - |     - |      24 B |
|   IMMapFold | 10000 |   239,564.8 ns |  4,673.56 ns |  5,739.55 ns |       - |       - |     - |      24 B |
|      MapMap | 10000 |   222,304.7 ns |  4,295.62 ns |  5,275.41 ns | 35.1563 | 16.8457 |     - |  295976 B |
|    IMMapMap | 10000 | 1,283,236.3 ns | 24,502.80 ns | 26,217.73 ns | 39.0625 | 19.5313 |     - |  331521 B |
|   MapFilter | 10000 |   108,468.9 ns |  2,028.20 ns |  1,991.96 ns |  6.8359 |  0.2441 |     - |   58336 B |
| IMMapFilter | 10000 |   250,490.2 ns |  5,009.87 ns |  4,920.36 ns |       - |       - |     - |      80 B |

</details>