```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7840/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i5-12450H 2.00GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 8.0.400
  [Host]     : .NET 8.0.8 (8.0.8, 8.0.824.36612), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.8 (8.0.8, 8.0.824.36612), X64 RyuJIT x86-64-v3


```
| Method                     | Mean       | Error     | StdDev    | Gen0     | Gen1    | Allocated  |
|--------------------------- |-----------:|----------:|----------:|---------:|--------:|-----------:|
| StringConcatenation        | 161.170 μs | 0.3290 μs | 0.2917 μs | 642.0898 | 12.9395 | 3933.56 KB |
| StringBuilderConcatenation |   1.336 μs | 0.0116 μs | 0.0109 μs |   2.6875 |  0.0877 |   16.47 KB |
