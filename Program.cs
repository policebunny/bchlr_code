using System;
using System.Collections.Generic;
using System.Diagnostics;
using BenchmarkDotNet;
using System.Collections.Generic;
using impl_search.Search;
using impl_search.SetUp;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using BenchmarkDotNet.Running;

namespace impl_search
{
    using impl_search.Logging;
    using Microsoft.Diagnostics.NETCore.Client;
    using OpenTK.Windowing.GraphicsLibraryFramework;
    using Visual;

    public class Entrypoint
    {

        static int numberOfMiliSeconds = 100; // Kleine Verzögerung zwischen dem Loggen, damit der Logvorgang beenden kann, bevor er erneut versucht auf dieselbe Datei zuzugreifen
        static VisualizeAlg vis = new VisualizeAlg();
        static void Main()
        {
            // Angabe ob manuelles Logging an (true) oder aus (false) sein soll
            bool logging = false; 

            // Angabe wieviele Grids verwendet werden sollen fürs Logging -> {0,1,2,3,4,5,6,7}
            int howMany = 7;

            // Angabe welches Grid gerade für die Visualisierung betrachtet werden soll -> {0,1,2,3,4,5,6,7}
            int whichVis = 2;


            // Benchmark-Tests
            // BenchmarkRunner.Run<SortBenchmark>();


            // Manuelles Logging
            // for(int i = 0; i <= howMany; i++) { runGrids(logging, i); }


            // Visualisierung der Grids
            // visGrids(logging, whichVis);


            // Visualisierung von CnC Schrittweise
            // RunCnCDia.SetUpCnCOG();
            // RunCnCDia.CnC_steps();
            // vis.visualizeCnCOG_steps();
            // vis = new VisualizeAlg();


            // Custom editieren von einem Grid
            /*
            CustomMap map = Logger.loadGrid();
            map.end = new Location(33,51);
            map.start = new Location(68,48);
            Logger.saveGrid(map);

            vis.visualizeEditable();
            vis = new VisualizeAlg();
            */

        }

        static void runGrids(bool logging, int whichGrid)
        {
            Console.WriteLine("RunA Dia --- ");
            RunAStarDia.SetUpAStarBig(whichGrid);

            RunAStarDia.GoA(logging);
            // vis.visualizeAStarDia();
            Thread.Sleep(numberOfMiliSeconds);

            Console.WriteLine("Run CNC Dia OG ---");

            RunCnCDia.SetUpCnCBig(whichGrid);

            RunCnCDia.GoCnCOG(logging);
            Thread.Sleep(numberOfMiliSeconds);
            Console.WriteLine("Now JPS running");
            RunJPS.SetUpJPSBig(whichGrid);

            RunJPS.GoJPS(logging);
            Thread.Sleep(numberOfMiliSeconds);
        }

        static void visGrids(bool logging, int whichVis)
        {
            RunAStarDia.SetUpAStarBig(whichVis);
            RunAStarDia.GoA(logging);
            vis.visualizeAStarDia();
            vis = new VisualizeAlg();

            RunJPS.SetUpJPSBig(whichVis);
            RunJPS.GoJPS(logging);
            vis.visualizeJPS();
            vis = new VisualizeAlg();

            RunCnCDia.SetUpCnCBig(whichVis);
            RunCnCDia.GoCnCOG(logging);
            vis.visualizeCnCOG();
            vis = new VisualizeAlg();
        }


    }
    
    
}

