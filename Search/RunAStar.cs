using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iced.Intel;
using impl_search.Logging;
using impl_search.SetUp;
using impl_search.Visual;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace impl_search.Search
{
    /*
    public class RunAStar
    {
        static SquareGrid grid = new SquareGrid(10, 10);
        static AStarSearch astar = new AStarSearch(grid, new Location(1, 4), new Location(8, 5));
        static Location start = new Location(0,0);
        static Location end = new Location(0,0);
        static void DrawGrid(SquareGrid grid, AStarSearch astar)
        {

            // Print out the cameFrom array
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    Location id = new Location(x, y);
                    Location ptr = id;
                    if (!astar.cameFrom.TryGetValue(id, out ptr))
                    {
                        ptr = id;
                    }
                    if (grid.walls.Contains(id)) { Console.Write("##"); }
                    else if (ptr.x == x + 1 && ptr.y == y) { Console.Write("> "); } //u2192 = B rechts
                    else if (ptr.x == x - 1 && ptr.y == y) { Console.Write("< "); } // u2190 = C links
                    else if (ptr.y == y + 1 && ptr.x == x) { Console.Write("v "); } // u2193 = D   runter
                    else if (ptr.y == y - 1 && ptr.x == x) { Console.Write("^ "); } //u2191 = A hoch

                    else { Console.Write("* "); }
                }
                Console.WriteLine();
            }
        }

        static void SetUpAGrid()
        {
            // Make "diagram 4" from main article
            
            grid = new SquareGrid(10, 10);
            for (var x = 1; x < 4; x++)
            {
                for (var y = 7; y < 9; y++)
                {
                    grid.walls.Add(new Location(x, y));
                }
            }
            grid.forests = new HashSet<Location>
                {
                    new Location(3, 4), new Location(3, 5),
                    new Location(4, 1), new Location(4, 2),
                    new Location(4, 3), new Location(4, 4),
                    new Location(4, 5), new Location(4, 6),
                    new Location(4, 7), new Location(4, 8),
                    new Location(5, 1), new Location(5, 2),
                    new Location(5, 3), new Location(5, 4),
                    new Location(5, 5), new Location(5, 6),
                    new Location(5, 7), new Location(5, 8),
                    new Location(6, 2), new Location(6, 3),
                    new Location(6, 4), new Location(6, 5),
                    new Location(6, 6), new Location(6, 7),
                    new Location(7, 3), new Location(7, 4),
                    new Location(7, 5)
                };
           


        }

        public static void SetUpAStar()
        {
            // generate map
            SetUpAGrid();
            // start = gen.getStartPositionOG(grid);
            // end = gen.getEndPositionOG(grid);
            // do we need more?
        }

        // int which = which grid type to make
        // 1 = grid type 1
        // 2 = grid type 2
        // 3 = grid type 3
        // 4 = grid type 4
        public static void SetUpAStarBig(int which)
        {
            switch(which)
            {
                case 1:
                    grid = BigGrid.makeGrid1();
                    break;
                case 2:
                    grid = BigGrid.makeGrid2();
                    break;
                case 3:
                    grid = BigGrid.makeGrid3();
                    break;
                case 4:
                    grid = BigGrid.makeGrid4();
                    break;
            }
        }


        public static void GoA()
        {
            start = new Location(1, 4);
            end = new Location(8, 5);

            Console.WriteLine("Loggin hier, zuerst ticks");
            for(int i = 0; i < 5; i++)
            {
                Information testinfo = testTicks();
                Logger.writeResults(testinfo);
            }
            Logger.writeEmptyResult();
            Console.WriteLine("Nun die Memory Tests");
            for (int i = 0;i < 5;i++)
            {
                Information testinfo = testMemory();
                Logger.writeResults(testinfo);
            }
            Console.WriteLine("Logging fertig");

            var timer = new Stopwatch();
            timer.Start();

            // EIGENTLICHER MESSPUNKT EINSTICH HIER
            long mem1 = GC.GetTotalMemory(false);
            astar = new AStarSearch(grid, new Location(1, 4),
                                        new Location(8, 5));
            // astar = new AStarSearch(grid, start, end);
            long mem2 = GC.GetTotalMemory(false);

            timer.Stop();

            long truemem = mem2 - mem1;
            
            Console.WriteLine("Memory: " + truemem);

            TimeSpan timeTakenA = timer.Elapsed;

            string fooA = "Time taken: " + timeTakenA.ToString(@"m\:ss\.fff");
            //Console.WriteLine(fooA);
            Console.WriteLine("Ticks taken: " + timeTakenA.Ticks);

            int nodes = astar.cameFrom.Count;
            Console.WriteLine("How many Nodes in cameFrom: " + nodes);
            string algA = "AStarOG";
            string focus = "CodeTest";
            Information info = new Information{
                whichAlg = algA,
                focusTest = focus,
                Date = DateTime.Parse("2025-03-14"),
                nodesTouched = nodes,
                memoryUsed = truemem,
                ticksTaken = timeTakenA.Ticks
            };
            Logger.writeResults(info);
            

            // VISUALIZER
            DrawGrid(grid, astar);
            // GoWindow();

        }


        static Information testTicks()
        {
            var timer = new Stopwatch();
            timer.Start();
            astar = new AStarSearch(grid, start, end);
            timer.Stop();
            TimeSpan timeTakenT = timer.Elapsed;
            string algA = "AStarOG";
            string focus = "testTicks";
            var path = ReconstructPath(astar.cameFrom, start, end);
            int pLength = path.Count;
            Information info = new Information
            {
                whichAlg = algA,
                focusTest = focus,
                Date = DateTime.Parse("2025-03-14"),
                nodesTouched = astar.cameFrom.Count,
                pathLength = pLength,
                memoryUsed = 0,
                ticksTaken = timeTakenT.Ticks
            };
            return info;
        }

        static Information testMemory()
        {
            long dif = 0;
            long mem1 = GC.GetTotalMemory(false);
            astar = new AStarSearch(grid, start, end);
            long mem2 = GC.GetTotalMemory(false);
            dif = mem2 - mem1;
            string algA = "AStarOG";
            string focus = "testMemory";
            var path = ReconstructPath(astar.cameFrom, start, end);
            int pLength = path.Count;
            Information info = new Information
            {
                whichAlg = algA,
                focusTest = focus,
                Date = DateTime.Parse("2025-03-14"),
                nodesTouched = astar.cameFrom.Count,
                pathLength = pLength,
                memoryUsed = dif,
                ticksTaken = 0
            };
            return info;
        }

        public static List<Location> ReconstructPath(Dictionary<Location, Location> cameFrom, Location start, Location goal)
        {
            var path = new List<Location>();

            if (!cameFrom.ContainsKey(goal))
                return path; // no path found

            Location current = goal;
            while (!current.Equals(start))
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Add(start);
            path.Reverse();

            return path;
        }

        public static AStarSearch getAStar()
        {
            return astar;
        }
        public static SquareGrid getAGrid()
        {
            return grid;
        }

        public static Location getStart()
        {
            return start;
        }

        public static Location getEnd()
        {
            return end;
        }
    }
    */
}
