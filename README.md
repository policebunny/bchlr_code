# Bachelorarbeit Vergleichsanalyse Code

Hier finden Sie einige Erklärungen zum Ausführen der einzelnen Codeabschnitte von diesem Projekt.

## Dependencies

Folgende Nuget-Pakete wurden installiert:
- BenchmarkDotNet
- Newtonsoft.Json
- OpenTK

## Struktur

Das Projekt wird von "Program.cs" aus gestartet.
Geloggte Daten sind unter dem Ordner "Logging" zu finden, bzw. die geloggten Daten für die Arbeit unter "Logging/data" zu finden.
Der vom Benchmark Framework generierte Output ist unter "BenchmarkDotNet.Artifacts" zu finden, bzw. die Daten für die Arbeit sind dort ebenfalls vermerkt.

## Einzelne Features

Innerhalb von "Program.cs" sind momentan alle möglichen Features auskommentiert, nach Bedarf können einzelne ausprobiert werden.
Zu jederzeit können die erstellten Fenster für die Visualisierung mit "Escape" wieder beendet werden.

Es folgt eine Erklärung:

```
            // Benchmark-Tests
            // BenchmarkRunner.Run<SortBenchmark>();
```
Auskommentieren um die Benchmark-Tests laufen zu lassen. 
Dabei muss das Projekt einmal mit "dotnet build" gebuildet und dann mit "dotnet run -c Release" ausgeführt werden.


```
            // Manuelles Logging
            // for(int i = 0; i <= howMany; i++) { runGrids(logging, i); }
```
Auskommentieren um die Algorithmen innerhalb der Grid mit dem manuellen Logging durchzuführen.
Hierbei werden die Werte "Checked Nodes" und "Path Length" gemessen und automatisch geloggt.
Die Ergebnisse werden in "loggedInfo.json" unter "Logging" gespeichert, wobei ein erneuter Logvorgang an die Datei angehangen wird.


```
            // Visualisierung der Grids
            // visGrids(logging, whichVis);
```
Auskommentieren um eine Visualisierung der Grids in einem bestimmten Grid anzuzeigen.
Es werden die Algorithmen nacheinander visualisiert, mit "Leertaste" wird der nächste Algorithmus angezeigt.


```
            // Visualisierung von CnC Schrittweise
            // RunCnCDia.SetUpCnCOG();
            // RunCnCDia.CnC_steps();
            // vis.visualizeCnCOG_steps();
            // vis = new VisualizeAlg();
```
Auskommentieren um eine Visualisierung vom CnC Algorithmus Schrittweise zu bekommen.
Hier wartet der Code bis "Leertaste" gedrückt wird, bevor der nächste Schritt angezeigt wird.


```
            // Custom editieren von einem Grid
            /*
            CustomMap map = Logger.loadGrid();
            map.end = new Location(33,51);
            map.start = new Location(68,48);
            Logger.saveGrid(map);

            vis.visualizeEditable();
            vis = new VisualizeAlg();
            */
```
Auskommentieren um das Beispielgrid manuell zu editieren. Dabei kann in das Grid hineingeklickt werden.
Mit "Leertaste" wird die Visualisierung beendet und das editierte Grid mit den Änderungen automatisch geloggt und als "test.json" unter "Logging" auffindbar.

