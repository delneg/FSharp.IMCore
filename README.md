# FSharp.IMCore


This repo hosts opinionated view on F# usage of System.Collections.Immutable

Benchmarks showed that sometimes System.Collections.Immutable datastructures can be faster than ones implemented in FSharp.Core.

This library tries to provide the same API as the standard collection (aside for the `IM` prefix),
in order to be able to simply replace i.e. `Set` with `IMSet` in the whole project and make no other changes - with some perfomance boost gained.

As of right now, the project is in very early stage - contributions are welcome, and please use with caution.
I.e. IMMap vs Map perfomance is relatively poor for most cases, and IMList vs List is downright awful - I guess,
recursive collections in FSharp.Core are heavily optimized.


Benchmarks will be added below for the reference of which structures to use in which cases.


For the not opinionated bindings to System.Collection.Immutable, please check out https://github.com/fsprojects/FSharp.Collections.Immutable


<details>
<summary>Set vs IMSet</summary>

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100
[Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT DEBUG
DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT


|          Method |     N |              Mean |           Error |         StdDev |     Gen 0 |    Gen 1 |   Gen 2 |   Allocated |
|---------------- |------ |------------------:|----------------:|---------------:|----------:|---------:|--------:|------------:|
|       SetForAll |  1000 |          8.020 ns |       0.0319 ns |      0.0299 ns |         - |        - |       - |           - |
|     IMSetForAll |  1000 |        150.481 ns |       0.8861 ns |      0.8288 ns |    0.0076 |        - |       - |        64 B |
|   SetDifference |  1000 |    231,872.259 ns |   2,678.5253 ns |  2,505.4943 ns |   55.1758 |   9.0332 |       - |   461,512 B |
| IMSetDifference |  1000 |    168,645.868 ns |     697.2205 ns |    652.1805 ns |         - |        - |       - |           - |
|         SetFold |  1000 |      2,708.537 ns |      29.8207 ns |     24.9016 ns |         - |        - |       - |           - |
|       IMSetFold |  1000 |     15,546.811 ns |     102.7669 ns |     96.1282 ns |         - |        - |       - |           - |
|          SetMap |  1000 |    384,138.285 ns |   7,335.7615 ns |  6,861.8759 ns |   74.2188 |  24.4141 |       - |   621,105 B |
|        IMSetMap |  1000 |    797,196.191 ns |  10,926.2664 ns |  9,685.8506 ns |   17.5781 |   5.8594 |       - |   152,792 B |
|       SetFilter |  1000 |    104,441.320 ns |   1,098.4122 ns |    973.7138 ns |   29.1748 |   3.5400 |       - |   245,000 B |
|     IMSetFilter |  1000 |     50,535.280 ns |     375.4094 ns |    351.1582 ns |    3.7231 |   0.3662 |       - |    31,640 B |
|        SetUnion |  1000 |    150,804.203 ns |     443.2385 ns |    370.1243 ns |   36.1328 |   9.0332 |       - |   304,064 B |
|      IMSetUnion |  1000 |    316,005.133 ns |   2,918.1579 ns |  2,729.6467 ns |   10.7422 |   2.9297 |       - |    89,984 B |
|    SetIntersect |  1000 |     59,462.554 ns |     221.5817 ns |    196.4264 ns |         - |        - |       - |        40 B |
|  IMSetIntersect |  1000 |     67,757.834 ns |     157.1665 ns |    147.0137 ns |         - |        - |       - |           - |
|    SetSingleton |  1000 |         28.645 ns |       0.1242 ns |      0.0969 ns |    0.0124 |        - |       - |       104 B |
|  IMSetSingleton |  1000 |         37.657 ns |       0.1587 ns |      0.1485 ns |    0.0134 |        - |       - |       112 B |
|       SetMinMax |  1000 |         74.128 ns |       0.3608 ns |      0.3375 ns |    0.0172 |        - |       - |       144 B |
|     IMSetMinMax |  1000 |         34.106 ns |       0.2345 ns |      0.2194 ns |    0.0057 |        - |       - |        48 B |
|       SetForAll | 10000 |          7.856 ns |       0.0347 ns |      0.0325 ns |         - |        - |       - |           - |
|     IMSetForAll | 10000 |        154.674 ns |       0.9335 ns |      0.8276 ns |    0.0076 |        - |       - |        64 B |
|   SetDifference | 10000 |  3,455,302.214 ns |  20,606.8952 ns | 19,275.7025 ns |  742.1875 | 148.4375 |       - | 6,234,811 B |
| IMSetDifference | 10000 |  2,154,180.301 ns |   6,247.3058 ns |  5,538.0738 ns |         - |        - |       - |         3 B |
|         SetFold | 10000 |     57,391.892 ns |     320.4262 ns |    299.7269 ns |         - |        - |       - |           - |
|       IMSetFold | 10000 |    160,018.808 ns |   1,046.7981 ns |    927.9592 ns |         - |        - |       - |           - |
|          SetMap | 10000 |  7,041,036.042 ns | 104,028.7302 ns | 97,308.5388 ns | 1000.0000 | 429.6875 | 54.6875 | 7,923,256 B |
|        IMSetMap | 10000 | 11,108,164.174 ns |  32,499.7667 ns | 28,810.1967 ns |  187.5000 |  78.1250 | 15.6250 | 1,622,255 B |
|       SetFilter | 10000 |  1,687,053.568 ns |  10,144.3267 ns |  9,489.0095 ns |  398.4375 | 199.2188 |       - | 3,343,657 B |
|     IMSetFilter | 10000 |    515,340.445 ns |   1,340.4538 ns |  1,119.3396 ns |   43.9453 |  20.5078 |       - |   371,577 B |
|        SetUnion | 10000 |  2,448,743.620 ns |  31,075.0510 ns | 29,067.6220 ns |  367.1875 | 183.5938 |       - | 3,093,187 B |
|      IMSetUnion | 10000 |  3,847,398.798 ns |  15,673.7260 ns | 13,088.2711 ns |  105.4688 |  50.7813 |       - |   900,227 B |
|    SetIntersect | 10000 |    737,531.966 ns |   4,515.4139 ns |  4,223.7209 ns |         - |        - |       - |        41 B |
|  IMSetIntersect | 10000 |    824,574.679 ns |   5,487.3932 ns |  4,864.4311 ns |         - |        - |       - |         1 B |
|    SetSingleton | 10000 |         28.237 ns |       0.2009 ns |      0.1781 ns |    0.0124 |        - |       - |       104 B |
|  IMSetSingleton | 10000 |         37.984 ns |       0.1298 ns |      0.1150 ns |    0.0134 |        - |       - |       112 B |
|       SetMinMax | 10000 |         89.220 ns |       0.3781 ns |      0.3537 ns |    0.0172 |        - |       - |       144 B |
|     IMSetMinMax | 10000 |         49.791 ns |       0.1580 ns |      0.1400 ns |    0.0057 |        - |       - |        48 B |


</details>

<details>
<summary>Map vs IMMap</summary>


BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100
[Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT DEBUG
DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT


|      Method |     N |            Mean |        Error |       StdDev |   Gen 0 |   Gen 1 | Allocated |
|------------ |------ |----------------:|-------------:|-------------:|--------:|--------:|----------:|
|   MapForAll |  1000 |        31.15 ns |     0.164 ns |     0.153 ns |       - |       - |         - |
| IMMapForAll |  1000 |       452.33 ns |     2.220 ns |     1.968 ns |  0.0210 |       - |     176 B |
|     MapFold |  1000 |     1,821.75 ns |     3.528 ns |     3.128 ns |       - |       - |         - |
|   IMMapFold |  1000 |    61,177.12 ns |   704.359 ns |   658.858 ns |       - |       - |         - |
|      MapMap |  1000 |    41,544.50 ns |   152.389 ns |   135.089 ns |  9.7046 |  2.2583 |  81,624 B |
|    IMMapMap |  1000 |   273,201.98 ns |   639.214 ns |   533.773 ns | 10.7422 |  3.4180 |  91,568 B |
|   MapFilter |  1000 |    13,948.35 ns |    69.409 ns |    61.529 ns |  2.6093 |  0.0458 |  21,928 B |
| IMMapFilter |  1000 |    75,294.89 ns | 1,167.768 ns | 1,092.331 ns |       - |       - |      56 B |
|   MapForAll | 10000 |        27.20 ns |     0.142 ns |     0.133 ns |       - |       - |         - |
| IMMapForAll | 10000 |       362.57 ns |     2.867 ns |     2.542 ns |  0.0210 |       - |     176 B |
|     MapFold | 10000 |     7,165.77 ns |    58.155 ns |    51.553 ns |       - |       - |         - |
|   IMMapFold | 10000 |   207,806.41 ns | 2,343.916 ns | 2,077.820 ns |       - |       - |         - |
|      MapMap | 10000 |   176,425.99 ns |   924.872 ns |   865.126 ns | 35.4004 | 17.5781 | 297,385 B |
|    IMMapMap | 10000 | 1,130,436.90 ns | 3,086.059 ns | 2,735.711 ns | 39.0625 | 19.5313 | 332,505 B |
|   MapFilter | 10000 |    46,153.60 ns |   153.448 ns |   143.535 ns |  6.7749 |  0.3052 |  56,784 B |
| IMMapFilter | 10000 |   253,876.10 ns | 3,118.289 ns | 2,916.850 ns |       - |       - |      56 B |

</details>

<details>
<summary>List vs IMList</summary>
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT DEBUG
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT


|          Method |     N |             Mean |          Error |         StdDev |    Gen 0 |   Gen 1 |   Allocated |
|---------------- |------ |-----------------:|---------------:|---------------:|---------:|--------:|------------:|
|      ListForAll |  1000 |         5.141 ns |      0.0224 ns |      0.0210 ns |        - |       - |           - |
|    IMListForAll |  1000 |       151.882 ns |      1.0145 ns |      0.8993 ns |   0.0105 |       - |        88 B |
|      ListChoose |  1000 |     8,736.220 ns |     72.1158 ns |     60.2199 ns |   3.3569 |  0.3357 |    28,112 B |
|    IMListChoose |  1000 |    69,056.106 ns |    447.1219 ns |    418.2381 ns |   4.2725 |  0.6104 |    36,576 B |
|   IMListChoose2 |  1000 |    48,040.123 ns |    221.8774 ns |    196.6886 ns |   6.0425 |  0.6714 |    50,752 B |
|      ListFilter |  1000 |     5,846.094 ns |     21.2049 ns |     19.8351 ns |   1.8845 |  0.1831 |    15,808 B |
|    IMListFilter |  1000 |   134,777.822 ns |    835.6174 ns |    781.6370 ns |   3.6621 |  0.2441 |    31,024 B |
|        ListSkip |  1000 |         6.610 ns |      0.0384 ns |      0.0359 ns |        - |       - |           - |
|      IMListSkip |  1000 |       818.328 ns |      3.2522 ns |      2.8830 ns |   0.0715 |       - |       600 B |
|     ListSplitAt |  1000 |       698.653 ns |      5.3646 ns |      4.4796 ns |   0.3862 |  0.0086 |     3,232 B |
|   IMListSplitAt |  1000 |    86,048.621 ns |    363.2984 ns |    339.8296 ns |   5.4932 |       - |    46,208 B |
|        ListFold |  1000 |     1,432.678 ns |      1.8324 ns |      1.7140 ns |        - |       - |           - |
|      IMListFold |  1000 |    10,491.300 ns |     19.1800 ns |     16.0162 ns |        - |       - |           - |
|      ListReduce |  1000 |     1,439.370 ns |      0.8971 ns |      0.7491 ns |        - |       - |           - |
|    IMListReduce |  1000 |    31,389.387 ns |    124.8516 ns |    110.6777 ns |        - |       - |       160 B |
|         ListMap |  1000 |    64,014.371 ns |    205.1201 ns |    191.8695 ns |  14.3433 |  4.7607 |   119,976 B |
|       IMListMap |  1000 |   112,953.759 ns |    366.0727 ns |    342.4246 ns |  17.2119 |  5.6152 |   144,168 B |
|      ListAppend |  1000 |     7,731.106 ns |     44.3695 ns |     39.3324 ns |   3.8223 |  0.6332 |    32,000 B |
|    IMListAppend |  1000 |       941.410 ns |      5.3253 ns |      4.9813 ns |   0.1211 |       - |     1,016 B |
|   ListSingleton |  1000 |        15.146 ns |      0.0996 ns |      0.0931 ns |   0.0076 |       - |        64 B |
| IMListSingleton |  1000 |        33.127 ns |      0.2342 ns |      0.2191 ns |   0.0124 |       - |       104 B |
|         ListSum |  1000 |       954.626 ns |      3.0562 ns |      2.7092 ns |        - |       - |           - |
|       IMListSum |  1000 |     9,699.323 ns |     22.5083 ns |     21.0543 ns |        - |       - |           - |
|       ListSumBy |  1000 |       947.559 ns |      0.5635 ns |      0.4705 ns |        - |       - |           - |
|     IMListSumBy |  1000 |     9,882.157 ns |     23.8627 ns |     21.1537 ns |        - |       - |           - |
|    ListContains |  1000 |    35,939.900 ns |    262.9516 ns |    245.9650 ns |   5.7373 |       - |    48,000 B |
|  IMListContains |  1000 |     4,563.146 ns |     23.2051 ns |     20.5707 ns |        - |       - |           - |
|        ListInit |  1000 |   111,061.821 ns |    309.8726 ns |    289.8550 ns |   3.7842 |  0.6104 |    32,000 B |
|      IMListInit |  1000 |   219,319.888 ns |    464.6621 ns |    434.6452 ns |   5.6152 |  0.7324 |    48,048 B |
|     ListTryFind |  1000 |        61.812 ns |      0.1517 ns |      0.1345 ns |   0.0029 |       - |        24 B |
|   IMListTryFind |  1000 |       172.524 ns |      1.5795 ns |      1.4002 ns |   0.0134 |       - |       112 B |
|     ListTryPick |  1000 |     1,922.144 ns |      1.0478 ns |      0.9801 ns |        - |       - |           - |
|   IMListTryPick |  1000 |   168,058.045 ns |  1,271.0858 ns |  1,188.9745 ns |  38.8184 |       - |   325,944 B |
|      ListForAll | 10000 |         5.136 ns |      0.0494 ns |      0.0413 ns |        - |       - |           - |
|    IMListForAll | 10000 |       173.922 ns |      1.2056 ns |      1.0687 ns |   0.0105 |       - |        88 B |
|      ListChoose | 10000 |   115,271.809 ns |    566.4800 ns |    529.8857 ns |  33.6914 | 12.8174 |   282,408 B |
|    IMListChoose | 10000 |   880,358.050 ns |    825.0980 ns |    731.4279 ns |  41.9922 | 19.5313 |   357,697 B |
|   IMListChoose2 | 10000 |   498,388.180 ns |  1,031.1969 ns |    914.1292 ns |  55.1758 | 25.3906 |   463,200 B |
|      ListFilter | 10000 |    83,596.041 ns |    269.9257 ns |    239.2821 ns |  19.0430 |  6.5918 |   159,776 B |
|    IMListFilter | 10000 | 1,753,234.841 ns |  4,045.1692 ns |  3,377.8995 ns |  41.0156 | 13.6719 |   343,746 B |
|        ListSkip | 10000 |         6.918 ns |      0.0881 ns |      0.0824 ns |        - |       - |           - |
|      IMListSkip | 10000 |     1,199.751 ns |      8.0230 ns |      7.1122 ns |   0.1049 |       - |       888 B |
|     ListSplitAt | 10000 |       800.292 ns |     10.3089 ns |      9.6430 ns |   0.3862 |  0.0086 |     3,232 B |
|   IMListSplitAt | 10000 | 1,149,259.152 ns |  3,422.2064 ns |  3,033.6969 ns |  56.6406 |       - |   478,929 B |
|        ListFold | 10000 |    14,288.402 ns |      7.4769 ns |      6.9939 ns |        - |       - |           - |
|      IMListFold | 10000 |   112,844.249 ns |    719.5815 ns |    673.0970 ns |        - |       - |           - |
|      ListReduce | 10000 |    14,298.859 ns |     17.7892 ns |     16.6400 ns |        - |       - |           - |
|    IMListReduce | 10000 |   322,432.407 ns |  1,101.1840 ns |    919.5386 ns |        - |       - |       160 B |
|         ListMap | 10000 |   891,235.176 ns |  3,757.9560 ns |  3,515.1943 ns | 142.5781 | 71.2891 | 1,199,547 B |
|       IMListMap | 10000 | 1,474,171.791 ns |  4,054.3294 ns |  3,594.0574 ns | 171.8750 | 85.9375 | 1,439,713 B |
|      ListAppend | 10000 |    91,426.165 ns |    327.0978 ns |    305.9675 ns |  38.2080 | 14.4043 |   320,000 B |
|    IMListAppend | 10000 |     1,665.017 ns |      3.3731 ns |      3.1552 ns |   0.1717 |       - |     1,448 B |
|   ListSingleton | 10000 |        14.859 ns |      0.0384 ns |      0.0341 ns |   0.0076 |       - |        64 B |
| IMListSingleton | 10000 |        31.912 ns |      0.1666 ns |      0.1558 ns |   0.0124 |       - |       104 B |
|         ListSum | 10000 |     9,293.217 ns |     11.7272 ns |      9.7928 ns |        - |       - |           - |
|       IMListSum | 10000 |   102,736.820 ns |    589.2565 ns |    551.1909 ns |        - |       - |           - |
|       ListSumBy | 10000 |     9,555.304 ns |     39.6765 ns |     30.9768 ns |        - |       - |           - |
|     IMListSumBy | 10000 |   106,860.774 ns |    889.1705 ns |    831.7307 ns |        - |       - |           - |
|    ListContains | 10000 |   350,438.906 ns |  3,373.5655 ns |  3,155.6353 ns |  57.1289 |       - |   480,000 B |
|  IMListContains | 10000 |    47,969.780 ns |    257.8587 ns |    228.5850 ns |        - |       - |           - |
|        ListInit | 10000 |   109,709.111 ns |    177.8927 ns |    166.4009 ns |   3.7842 |  0.6104 |    32,000 B |
|      IMListInit | 10000 |   220,245.344 ns |    266.3294 ns |    236.0941 ns |   5.6152 |  0.7324 |    48,048 B |
|     ListTryFind | 10000 |        15.452 ns |      0.0718 ns |      0.0672 ns |   0.0029 |       - |        24 B |
|   IMListTryFind | 10000 |       196.832 ns |      1.4203 ns |      1.3286 ns |   0.0134 |       - |       112 B |
|     ListTryPick | 10000 |    19,120.332 ns |      3.9165 ns |      3.4719 ns |        - |       - |           - |
|   IMListTryPick | 10000 | 2,280,770.898 ns | 17,818.7166 ns | 16,667.6386 ns | 574.2188 |       - | 4,807,707 B |
</details>