# dotNetPerfomanceTests

Linq select vs foreach and for  
Loop, extract info and store in hashset

.Net 5
Intel(R) Core(TM) i5-7300HQ CPU @ 2.50GHz   2.50 GHz
16.0 GB (15.9 GB usable)

|   Size  | LinqSelect | Foreach (multiple memory allocs) | Foreach (single memory alloc) | For |
|:-------:|:---------: | :------------------------------: | :---------------------------: | :-: |
| 100000  |   23ms     |            41ms                  |            25ms               | 5ms |

