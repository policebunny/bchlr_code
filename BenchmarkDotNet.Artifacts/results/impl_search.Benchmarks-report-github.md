```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7840/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]     : .NET 8.0.8 (8.0.8, 8.0.824.36612), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.8 (8.0.8, 8.0.824.36612), X64 RyuJIT x86-64-v3


```
| Method         | Mean      | Error    | StdDev   |
|--------------- |----------:|---------:|---------:|
| ForLoopSum     | 313.90 ns | 2.843 ns | 2.660 ns |
| ForeachLoopSum | 292.58 ns | 0.218 ns | 0.170 ns |
| LinqSelect     |  73.46 ns | 0.182 ns | 0.152 ns |
