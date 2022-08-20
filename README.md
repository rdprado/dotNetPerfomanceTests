# dotNetPerfomanceTests

## Linq select vs foreach and for  
Loop, extract info and store in hashset

.Net 5
Intel(R) Core(TM) i5-7300HQ CPU @ 2.50GHz   2.50 GHz
16.0 GB (15.9 GB usable)

|   Size  | Linq Select | Foreach \*1 | Foreach \*2 | For |
|:-------:|:---------: | :----------: | :---------: | :-: |
| 100000  |   23ms     |      41ms    |     25ms    | 5ms |

\*1 - Initialized hashset with no defined size (multiple menmory allocations)  
\*2 - Initialized hashset with pre defined size (single menmory allocations)

