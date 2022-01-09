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


BenchmarkDotNet=v0.12.1, OS=nixos 20.09.3072.d4c29df154d
AMD Ryzen 3 2200G with Radeon Vega Graphics, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.102
[Host]     : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT DEBUG
DefaultJob : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT



|      Method |     N |           Mean |       Error |      StdDev |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|------------ |------ |---------------:|------------:|------------:|--------:|--------:|------:|----------:|
|   MapForAll |  1000 |       135.0 ns |     0.48 ns |     0.40 ns |  0.0114 |       - |     - |      24 B |
| IMMapForAll |  1000 |       541.7 ns |     1.72 ns |     1.61 ns |  0.0954 |       - |     - |     200 B |
|     MapFold |  1000 |     8,544.7 ns |    15.31 ns |    14.32 ns |       - |       - |     - |      24 B |
|   IMMapFold |  1000 |   101,637.8 ns |   365.66 ns |   324.15 ns |       - |       - |     - |      25 B |
|      MapMap |  1000 |    83,155.9 ns |   510.71 ns |   477.72 ns | 37.7197 |  4.1504 |     - |   81712 B |
|    IMMapMap |  1000 |   480,002.0 ns | 3,065.20 ns | 2,393.10 ns | 34.6680 |  9.2773 |     - |   92040 B |
|   MapFilter |  1000 |    44,084.0 ns |   186.23 ns |   155.51 ns | 10.6812 |       - |     - |   22456 B |
| IMMapFilter |  1000 |   110,120.3 ns |   197.71 ns |   165.10 ns |       - |       - |     - |      80 B |
|   MapForAll | 10000 |       128.7 ns |     0.39 ns |     0.37 ns |  0.0114 |       - |     - |      24 B |
| IMMapForAll | 10000 |       758.6 ns |     7.64 ns |     7.14 ns |  0.0954 |       - |     - |     200 B |
|     MapFold | 10000 |    30,517.7 ns |   106.01 ns |    93.97 ns |       - |       - |     - |      24 B |
|   IMMapFold | 10000 |   336,326.8 ns |   544.78 ns |   509.59 ns |       - |       - |     - |      29 B |
|      MapMap | 10000 |   328,955.9 ns |   737.01 ns |   615.44 ns | 55.1758 | 24.4141 |     - |  294248 B |
|    IMMapMap | 10000 | 1,906,479.9 ns | 3,067.23 ns | 2,869.09 ns | 56.6406 | 27.3438 |     - |  338280 B |
|   MapFilter | 10000 |   127,451.6 ns |   209.50 ns |   195.97 ns | 27.0996 |       - |     - |   56738 B |
| IMMapFilter | 10000 |   355,830.2 ns |   642.02 ns |   536.12 ns |       - |       - |     - |      80 B |

</details>

<details>
<summary>List vs IMList</summary>
BenchmarkDotNet=v0.12.1, OS=nixos 20.09.3072.d4c29df154d
AMD Ryzen 3 2200G with Radeon Vega Graphics, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.102
  [Host]     : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT DEBUG
  DefaultJob : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

|          Method |     N |             Mean |          Error |         StdDev |           Median |     Gen 0 |    Gen 1 | Gen 2 | Allocated |
|---------------- |------ |-----------------:|---------------:|---------------:|-----------------:|----------:|---------:|------:|----------:|
|      ListForAll |  1000 |         7.981 ns |      0.0316 ns |      0.0280 ns |         7.970 ns |    0.0115 |        - |     - |      24 B |
|    IMListForAll |  1000 |       291.269 ns |      1.2885 ns |      1.2053 ns |       291.041 ns |    0.0534 |        - |     - |     112 B |
|      ListChoose |  1000 |    13,099.979 ns |     59.8523 ns |     49.9794 ns |    13,120.944 ns |   13.6108 |        - |     - |   28472 B |
|    IMListChoose |  1000 |   115,103.707 ns |    253.9486 ns |    225.1188 ns |   115,133.916 ns |   17.2119 |        - |     - |   36096 B |
|   IMListChoose2 |  1000 |    78,118.902 ns |    371.6425 ns |    347.6346 ns |    78,190.805 ns |   21.4844 |        - |     - |   44977 B |
|      ListFilter |  1000 |    10,583.587 ns |    187.5933 ns |    166.2966 ns |    10,546.906 ns |    7.6904 |        - |     - |   16088 B |
|    IMListFilter |  1000 |   227,469.048 ns |  1,090.5114 ns |  1,020.0651 ns |   226,954.808 ns |   14.6484 |        - |     - |   31002 B |
|        ListSkip |  1000 |        15.870 ns |      0.0386 ns |      0.0361 ns |        15.871 ns |         - |        - |     - |         - |
|      IMListSkip |  1000 |       999.828 ns |      1.9672 ns |      1.7439 ns |       999.532 ns |    0.2861 |        - |     - |     600 B |
|     ListSplitAt |  1000 |       835.706 ns |     16.2939 ns |     15.2413 ns |       829.343 ns |    1.5450 |        - |     - |    3232 B |
|   IMListSplitAt |  1000 |   109,252.525 ns |    281.4130 ns |    249.4653 ns |   109,235.328 ns |   22.0947 |        - |     - |   46209 B |
|        ListFold |  1000 |     1,655.325 ns |      0.4855 ns |      0.4054 ns |     1,655.340 ns |    0.0114 |        - |     - |      24 B |
|      IMListFold |  1000 |    36,502.616 ns |    167.2960 ns |    156.4888 ns |    36,460.795 ns |         - |        - |     - |      24 B |
|      ListReduce |  1000 |     1,781.776 ns |     35.0616 ns |     70.8261 ns |     1,817.561 ns |    0.0114 |        - |     - |      24 B |
|    IMListReduce |  1000 |    48,437.398 ns |    234.8416 ns |    219.6709 ns |    48,494.133 ns |    0.0610 |        - |     - |     184 B |
|         ListMap |  1000 |   124,002.923 ns |  2,285.7379 ns |  4,771.1861 ns |   123,511.850 ns |   40.5273 |  13.1836 |     - |  119992 B |
|       IMListMap |  1000 |   183,821.216 ns |    728.1183 ns |    681.0823 ns |   183,944.574 ns |   45.6543 |  14.4043 |     - |  144225 B |
|      ListAppend |  1000 |    12,014.536 ns |    239.5601 ns |    605.3989 ns |    12,221.564 ns |   15.2893 |        - |     - |   32000 B |
|    IMListAppend |  1000 |     1,318.064 ns |     26.3850 ns |     54.4896 ns |     1,282.044 ns |    0.4845 |        - |     - |    1016 B |
|   ListSingleton |  1000 |        19.883 ns |      0.1366 ns |      0.1278 ns |        19.838 ns |    0.0305 |        - |     - |      64 B |
| IMListSingleton |  1000 |        51.879 ns |      0.1992 ns |      0.1863 ns |        51.792 ns |    0.0496 |        - |     - |     104 B |
|         ListSum |  1000 |     1,181.593 ns |     10.0205 ns |      7.8234 ns |     1,182.945 ns |         - |        - |     - |         - |
|       IMListSum |  1000 |    31,356.484 ns |    188.7420 ns |    176.5494 ns |    31,443.292 ns |         - |        - |     - |         - |
|       ListSumBy |  1000 |     1,135.048 ns |     22.3855 ns |     26.6484 ns |     1,131.977 ns |         - |        - |     - |         - |
|     IMListSumBy |  1000 |    35,293.943 ns |     73.3893 ns |     68.6484 ns |    35,304.447 ns |         - |        - |     - |         - |
|    ListContains |  1000 |    39,835.361 ns |    270.3180 ns |    225.7277 ns |    39,792.437 ns |   22.9492 |        - |     - |   47999 B |
|  IMListContains |  1000 |    40,538.465 ns |    780.5541 ns |    730.1308 ns |    40,505.778 ns |         - |        - |     - |         - |
|        ListInit |  1000 |   191,722.961 ns |    204.6206 ns |    191.4022 ns |   191,755.327 ns |   15.1367 |        - |     - |   32027 B |
|      IMListInit |  1000 |   326,264.248 ns |  1,449.4267 ns |  1,355.7946 ns |   326,144.770 ns |   22.9492 |        - |     - |   48072 B |
|     ListTryFind |  1000 |        28.426 ns |      0.0975 ns |      0.0864 ns |        28.414 ns |    0.0229 |        - |     - |      48 B |
|   IMListTryFind |  1000 |       446.718 ns |      1.7390 ns |      1.5416 ns |       447.124 ns |    0.0648 |        - |     - |     136 B |
|     ListTryPick |  1000 |     1,943.221 ns |      0.6993 ns |      0.6199 ns |     1,943.181 ns |    0.0114 |        - |     - |      24 B |
|   IMListTryPick |  1000 |   217,044.028 ns |  1,530.3786 ns |  1,356.6407 ns |   217,415.574 ns |  155.7617 |        - |     - |  325969 B |
|      ListForAll | 10000 |         8.368 ns |      0.2016 ns |      0.2071 ns |         8.432 ns |    0.0115 |        - |     - |      24 B |
|    IMListForAll | 10000 |       329.538 ns |      1.2781 ns |      1.0673 ns |       329.340 ns |    0.0534 |        - |     - |     112 B |
|      ListChoose | 10000 |   212,257.603 ns |    546.8742 ns |    484.7897 ns |   212,096.262 ns |   60.3027 |  26.1230 |     - |  285010 B |
|    IMListChoose | 10000 | 1,395,521.969 ns |  3,398.8323 ns |  3,179.2698 ns | 1,394,403.982 ns |   62.5000 |  31.2500 |     - |  360384 B |
|   IMListChoose2 | 10000 |   837,826.524 ns |  3,308.0439 ns |  2,932.4948 ns |   838,107.884 ns |   86.9141 |  42.9688 |     - |  462585 B |
|      ListFilter | 10000 |   135,438.251 ns |    778.5044 ns |    728.2135 ns |   135,575.125 ns |   34.4238 |  15.8691 |     - |  162040 B |
|    IMListFilter | 10000 | 2,695,716.586 ns |  8,110.9267 ns |  7,190.1253 ns | 2,697,162.320 ns |   74.2188 |  31.2500 |     - |  340617 B |
|        ListSkip | 10000 |        15.873 ns |      0.0437 ns |      0.0408 ns |        15.863 ns |         - |        - |     - |         - |
|      IMListSkip | 10000 |     1,419.285 ns |      4.9647 ns |      4.6440 ns |     1,419.321 ns |    0.4234 |        - |     - |     888 B |
|     ListSplitAt | 10000 |       825.862 ns |     10.6224 ns |      9.9362 ns |       823.962 ns |    1.5450 |        - |     - |    3232 B |
|   IMListSplitAt | 10000 | 1,445,903.808 ns |  3,497.6628 ns |  3,100.5870 ns | 1,446,642.225 ns |  224.6094 |        - |     - |  478936 B |
|        ListFold | 10000 |    16,368.057 ns |      3.0471 ns |      2.3789 ns |    16,368.238 ns |         - |        - |     - |      24 B |
|      IMListFold | 10000 |   350,708.865 ns |  2,996.9287 ns |  2,502.5712 ns |   350,891.761 ns |         - |        - |     - |      24 B |
|      ListReduce | 10000 |    16,413.232 ns |     21.4287 ns |     20.0444 ns |    16,409.891 ns |         - |        - |     - |      24 B |
|    IMListReduce | 10000 |   474,313.334 ns |  1,187.0104 ns |    991.2075 ns |   474,254.945 ns |         - |        - |     - |     189 B |
|         ListMap | 10000 | 1,585,333.988 ns |  4,693.6992 ns |  4,390.4891 ns | 1,584,870.570 ns |  191.4063 |  95.7031 |     - | 1199617 B |
|       IMListMap | 10000 | 2,560,163.563 ns | 10,041.1592 ns |  9,392.5065 ns | 2,556,900.688 ns |  230.4688 | 113.2813 |     - | 1439754 B |
|      ListAppend | 10000 |   169,355.926 ns |    497.7907 ns |    465.6337 ns |   169,292.731 ns |   51.5137 |  25.6348 |     - |  320003 B |
|    IMListAppend | 10000 |     2,354.902 ns |      6.7771 ns |      6.0078 ns |     2,354.560 ns |    0.6905 |        - |     - |    1448 B |
|   ListSingleton | 10000 |        19.841 ns |      0.0797 ns |      0.0706 ns |        19.839 ns |    0.0305 |        - |     - |      64 B |
| IMListSingleton | 10000 |        52.006 ns |      0.6046 ns |      0.5048 ns |        51.870 ns |    0.0496 |        - |     - |     104 B |
|         ListSum | 10000 |    11,331.306 ns |     83.6743 ns |     78.2690 ns |    11,308.454 ns |         - |        - |     - |         - |
|       IMListSum | 10000 |   317,963.375 ns |    552.8468 ns |    461.6521 ns |   318,028.810 ns |         - |        - |     - |       3 B |
|       ListSumBy | 10000 |    11,073.473 ns |     78.3062 ns |     69.4164 ns |    11,057.138 ns |         - |        - |     - |         - |
|     IMListSumBy | 10000 |   319,061.164 ns |    272.1823 ns |    227.2846 ns |   319,113.925 ns |         - |        - |     - |         - |
|    ListContains | 10000 |   405,510.339 ns |  2,259.4064 ns |  2,113.4502 ns |   404,869.468 ns |  229.4922 |        - |     - |  480001 B |
|  IMListContains | 10000 |   357,344.977 ns |    415.4500 ns |    324.3560 ns |   357,347.705 ns |         - |        - |     - |       3 B |
|        ListInit | 10000 |   190,739.865 ns |    315.8135 ns |    295.4121 ns |   190,762.974 ns |   15.1367 |        - |     - |   32024 B |
|      IMListInit | 10000 |   323,333.401 ns |  1,270.9053 ns |  1,188.8056 ns |   323,405.339 ns |   22.9492 |        - |     - |   48072 B |
|     ListTryFind | 10000 |        20.761 ns |      0.0857 ns |      0.0801 ns |        20.764 ns |    0.0229 |        - |     - |      48 B |
|   IMListTryFind | 10000 |       530.851 ns |      1.2373 ns |      1.1574 ns |       531.061 ns |    0.0648 |        - |     - |     136 B |
|     ListTryPick | 10000 |    19,244.348 ns |      3.1145 ns |      2.7609 ns |    19,243.698 ns |         - |        - |     - |      24 B |
|   IMListTryPick | 10000 | 3,227,740.197 ns | 20,811.8336 ns | 19,467.4021 ns | 3,231,371.820 ns | 2296.8750 |        - |     - | 4807728 B |
</details>