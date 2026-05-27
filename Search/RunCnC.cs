using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using impl_search.SetUp;
using impl_search.Logging;

namespace impl_search.Search
{
    public class RunCnC
    {
        /*
        static SquareGrid grid = new SquareGrid(10, 10);
        static CnCCrippled cncOG = new CnCCrippled(grid, new Location(1, 4), new Location(8, 5));
        static Location start = new Location(0, 0);
        static Location end = new Location(0, 0);
        static void DrawGrid(SquareGrid grid, CnCCrippled cncOG)
        {

            Console.WriteLine("In draw grid cnc, vor for for");
            foreach(var loc in grid.walls)
            {
                Console.WriteLine("wall id, x: " + loc.x + ", y: " + loc.y);
            }

            // Print out the cameFrom array
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    Location id = new Location(x, y);
                    Location ptr = id;
                    if (cncOG.cameFrom.TryGetValue(id, out ptr))
                    {
                        ptr = id;
                    } else
                    {
                        ptr = new Location(-1, -1);
                    }
                    
                    // Console.WriteLine(ptr.x + ", !!!" + ptr.y );
                    if (grid.walls.Contains(id)) { Console.Write("##"); }
                    else if (ptr.x == x && ptr.y == y) { Console.Write ("p "); }

                    else { Console.Write("* "); }
                }
                Console.WriteLine();
            }
        }

        public static void SetUpCnCGrid()
        {
            
            grid = new SquareGrid(10, 10);

            for (int y = 3; y <= 6; y++)
            {
                int x = 4;
                grid.walls.Add(new Location(x,y));
            }
            

        }



        public static void SetUpCnC()
        {

            SetUpCnCGrid();
            // USE GENERATED INSTEAD
        }


        public static void GoCnC()
        {
            start = new Location(1, 4);
            end = new Location(8, 5);
            
            Logger.writeEmptyResult();
            Console.WriteLine("Loggin hier, zuerst ticks");
            for (int i = 0; i < 5; i++)
            {
                Information testinfo = testTicks();
                Logger.writeResults(testinfo);
            }
            Logger.writeEmptyResult();
            Console.WriteLine("Nun die Memory Tests");
            for (int i = 0; i < 5; i++)
            {
                Information testinfo = testMemory();
                Logger.writeResults(testinfo);
            }
            Console.WriteLine("Logging fertig");
            

            // VISUALOZER
            cncOG = new CnCCrippled(grid, new Location(1, 4), new Location(8, 5));
            DrawGrid(grid, cncOG);

        }

        static Information testTicks()
        {
            var timer = new Stopwatch();
            timer.Start();
            cncOG = new CnCCrippled(grid, start, end);
            timer.Stop();
            TimeSpan timeTakenT = timer.Elapsed;
            string algA = "cncOGOG";
            string focus = "testTicks";
            Information info = new Information
            {
                whichAlg = algA,
                focusTest = focus,
                Date = DateTime.Parse("2025-03-14"),
                nodesTouched = cncOG.cameFrom.Count,
                memoryUsed = 0,
                ticksTaken = timeTakenT.Ticks
            };
            return info;
        }

        static Information testMemory()
        {
            long dif = 0;
            long mem1 = GC.GetTotalMemory(false);
            Console.WriteLine("start: " + start.x + " " + start.y);
            cncOG = new CnCCrippled(grid, start, end);
            long mem2 = GC.GetTotalMemory(false);
            dif = mem2 - mem1;
            Console.WriteLine("In testmemory cnc");
            Console.WriteLine("mem1: " + mem1 + ", mem2: " + mem2 + ", dif: " + dif);
            string algA = "cncOGOG";
            string focus = "testMemory";
            Information info = new Information
            {
                whichAlg = algA,
                focusTest = focus,
                Date = DateTime.Parse("2025-03-14"),
                nodesTouched = cncOG.cameFrom.Count,
                memoryUsed = dif,
                ticksTaken = 0
            };
            return info;
        }
        
        */
    }
        
}
